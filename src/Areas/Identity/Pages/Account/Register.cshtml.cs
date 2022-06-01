using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace zmdh.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private DBManager _DBManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, DBManager _dbm)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _DBManager = _dbm;
        }

        [BindProperty]
        public ClientInput clientInput { get; set; }

        [BindProperty]
        public OuderInput ouderInput { get; set; }

        [BindProperty]
        public ApiInput apiInput { get; set; }

        public string ReturnUrl { get; set; }
        
        public int clientid { get; set; }
        public Aanmelding aanmelding { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class ClientInput
        {
            [Required(ErrorMessage = "Email is verplicht")]
            [EmailAddress(ErrorMessage = "Vul een geldig emailadres in")]
            [Display(Name = "Email*")]
            public string Email { get; set; }

            [MaxLength(30, ErrorMessage = "Maximale lengte is 20")]
            [MinLength(5, ErrorMessage = "Minimale lengte is 5")]
            [Required(ErrorMessage = "Adres is verplicht")]
            [Display(Name = "Adres*")]
            public string Adres { get; set; }

            [MaxLength(30, ErrorMessage = "Maximale lengte is 20")]
            [MinLength(5, ErrorMessage = "Minimale lengte is 5")]
            [Required(ErrorMessage = "Woonplaats is verplicht")]
            [Display(Name = "Woonplaats*")]
            public string Residence { get; set; }

            [Required(ErrorMessage = "Hulpverlener is verplicht")]
            [Display(Name = "Hulpverlener met wie u contact wilt*")]
            public int HulpverlenerId { get; set; }
        }

        public class OuderInput
        {
            [EmailAddress(ErrorMessage = "Vul een geldig emailadres in")]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [MaxLength(30, ErrorMessage = "Maximale lengte is 20")]
            [MinLength(5, ErrorMessage = "Minimale lengte is 5")]
            [Display(Name = "Voor en achternaam")]
            public string FullName { get; set; }

            [Display(Name = "Ouder/Verzorger gaat akkoord.")]
            public bool Agree { get; set; }
        }

        public class ApiInput
        {
            [MaxLength(30, ErrorMessage = "Maximale lengte is 20")]
            [MinLength(5, ErrorMessage = "Minimale lengte is 5")]
            [Required(ErrorMessage = "Voor en achternaam is verplicht")]
            [Display(Name = "Voor en achternaam")]
            public string FullName { get; set; }

            [MaxLength(9, ErrorMessage = "Maximale lengte is 9")]
            [MinLength(9, ErrorMessage = "Minimale lengte is 9")]
            [Required(ErrorMessage = "BSN is verplicht")]
            [Display(Name = "BSN*")]
            public string BSN { get; set; }

            [MaxLength(20, ErrorMessage = "Maximale lengte is 20")]
            [MinLength(15, ErrorMessage = "Minimale lengte is 15")]
            [Required(ErrorMessage = "Iban is verplicht")]
            [Display(Name = "Iban*")]
            public string Iban { get; set; }

            [Required(ErrorMessage = "Geboortedatum is verplicht")]
            [Display(Name = "Geboortedatum*")]
            public DateTime Birthdate { get; set; }
        }

        public class SpecificApiClient
        {
            public string volledigenaam { get; set; }
            public string IBAN { get; set; }
            public string BSN { get; set; }
            public string gebdatum { get; set; }
            public int clientid { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if(_DBManager.Hulpverleners.Any())
                ViewData["hulpverleners"] =  _DBManager.Hulpverleners.ToList();
                
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if(_DBManager.Hulpverleners.Any())
                ViewData["hulpverleners"] =  _DBManager.Hulpverleners.ToList();

            returnUrl = Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            //checken of alles is ingevuld en check om te kijken of de datum niet verder dan vandaag is.
            if(ModelState.IsValid && DateTime.Now.CompareTo(apiInput.Birthdate) == 1)
            {
                //check of email bestaat en of er data is ingevuld voor een ouder als client jonger dan 16 is. Eventueel vullen met viewdata.
                if(!CheckOuderInputUnderSixteen() && !CheckEmailExists())
                {
                    var clientids = await ClientApi.FetchClientIds();
                    await RegisterClient();

                    ViewData["error"] = null;

                    return Redirect(returnUrl);
                }
            }else{
                ViewData["error"] = "Date";
            }
            return Page();
        }

        public bool CheckOuderInputUnderSixteen()
        {
            if(!AboveSixteenCheck() && (ouderInput.Email == null || ouderInput.FullName == null))
            {
                ViewData["error"] = "UnderSixteen";
                return true;
            }

            return false;
        }

        public bool CheckEmailExists()
        {
            if(!_userManager.Users.Any(o => o.Email == ouderInput.Email) && !_userManager.Users.Any(o => o.Email == clientInput.Email))
                return false;

            if(_userManager.Users.Any(c => c.Email == ouderInput.Email))
                ViewData["error"] =  "Ouder";

            if(_userManager.Users.Any(c => c.Email == clientInput.Email))
                ViewData["error"] =  "Client";

            return true;
        }

        public bool AboveSixteenCheck()
        {
            var Yearsbetween = DateTime.Now.Year - apiInput.Birthdate.Year;
            var Monthsbetween = DateTime.Now.Month > apiInput.Birthdate.Month;
            var Daysbetween = DateTime.Now.Day > apiInput.Birthdate.Day;

            var sameDay = DateTime.Now.Day == apiInput.Birthdate.Day;
            var sameMonth = DateTime.Now.Month == apiInput.Birthdate.Month;

            if(Yearsbetween >= 16 || (Yearsbetween == 15 && (Daysbetween || Monthsbetween)) || (Yearsbetween == 15 && (sameDay && sameMonth)))
                return true;
            
            return false;
        }
        
        public async Task RegisterClient()
        {
            if(!_userManager.Users.Any(u => u.Email == ouderInput.Email))
            {
                clientid = await ClientApi.PostClient(apiInput);

                if(clientid != 0)
                {
                    var hulpverlener = _DBManager.Hulpverleners.SingleOrDefault(h => h.Id == clientInput.HulpverlenerId);
                    aanmelding = new Aanmelding();

                    //Client registratie
                    var user1 = new ApplicationUser { UserName = clientInput.Email, Email = clientInput.Email };
                    var client = new Client { ClientId = clientid, Adres = clientInput.Adres, Residence = clientInput.Residence, Hulpverlener = hulpverlener, Account = user1, AboveSixteen = AboveSixteenCheck()};

                    await _DBManager.Clienten.AddAsync(client);
                    var result1 = await _userManager.CreateAsync(user1);
                
                    if (result1.Succeeded)
                    {
                        _logger.LogInformation("Client account aangemaakt zonder wachtwoord.");
                        
                        await _userManager.AddToRoleAsync(user1, "Client");

                        //Ouder registratie
                        await RegisterOuder();

                        //Aanmelding initalizeren
                        aanmelding.Client = client;
                        aanmelding.Hulpverlener = hulpverlener;
                        aanmelding.Agree = ouderInput.Agree;

                        await _DBManager.Aanmeldingen.AddAsync(aanmelding);
                        _DBManager.SaveChanges();
                    }
                    foreach (var error in result1.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
        }

        public async Task RegisterOuder()
        {
            if(ouderInput.Email != null && ouderInput.FullName != null)
            {
                var user2 = new ApplicationUser { UserName = ouderInput.Email, Email = ouderInput.Email };
                var ouder = new Ouder { FullName = ouderInput.FullName, ClientId = clientid, Account = user2 };
                
                await _DBManager.Ouders.AddAsync(ouder);
                var result2 = await _userManager.CreateAsync(user2);

                if (result2.Succeeded)
                {
                    _logger.LogInformation("Ouder account aangemaakt zonder wachtwoord.");
                    
                    await _userManager.AddToRoleAsync(user2, "Ouder");

                    aanmelding.Ouder = ouder;
                }
                foreach (var error in result2.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
    }
}
