using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using static zmdh.Areas.Identity.Pages.Account.RegisterModel;

namespace zmdh.Controllers
{
    public class HulpverlenerController : Controller
    {
        public readonly DBManager _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public HulpverlenerController(DBManager context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }
        public IQueryable<Hulpverlener> Sorteer(IQueryable<Hulpverlener> Lijst, string Sorteer)
        {
            if (Sorteer == "naam_aflopend")
                return Lijst.OrderByDescending(s => s.Name.ToLower());

            if (Sorteer == "specialisatie_aflopend")
                return Lijst.OrderByDescending(s => s.Specialisatie.ToLower());

            if (Sorteer == "specialisatie_oplopend")
                return Lijst.OrderBy(s => s.Specialisatie.ToLower());

            return Lijst.OrderBy(s => s.Name.ToLower());
        }
        public IQueryable<Hulpverlener> Zoek(IQueryable<Hulpverlener> Lijst, string zoek)
        {
            if (zoek == null)
                return Lijst;
            else
                return Lijst.Where(s => s.Name.ToLower().Contains(zoek.ToLower()));
        }
        public IQueryable<Hulpverlener> Pagineer(IQueryable<Hulpverlener> Lijst, int pagina, int aantal)
        {
            if (pagina < 0)
            {
                pagina = 0;
            }
            return Lijst.Skip((pagina) * aantal).Take(aantal);
        }

        // GET: Hulpverlener
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Index(string sorteer, string zoek, int pagina)
        {
            if (sorteer == null) sorteer = "naam_oplopend";
            ViewData["Sorteer"] = sorteer;
            ViewData["pagina"] = pagina;
            ViewData["heeftVolgende"] = (pagina + 1) * 10 < _context.Hulpverleners.Count();
            ViewData["heeftVorige"] = pagina > 0;
            return View(await Pagineer(Zoek(Sorteer(_context.Hulpverleners, sorteer), zoek), pagina, 10).ToListAsync());
        }

        [Authorize(Roles = "Hulpverlener")]
        public async Task<IActionResult> Inbox()
        {
            var apiclients = new List<SpecificApiClient>();
            var user = await _usermanager.GetUserAsync(User);
            var aanmeldingenlijst = await _context.Aanmeldingen.Where(s => s.HulpverlenerId == user.HulpverlenerId).Include(s => s.Ouder).Include(s => s.Client).ToListAsync();
            foreach (var item in aanmeldingenlijst)
            {
                apiclients.Add(await ClientApi.PullSpecificClient(item.ClientId));
            }
            ViewData["apiclientslist"] = apiclients;

            return View(new SpecificApiClientViewModel() { aanmeldingen = aanmeldingenlijst, specificApiClient = apiclients });
        }

        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Clienten(string sorteer, string zoek, int pagina)
        {
            var apiclients = new List<SpecificApiClient>();
            var user = await _usermanager.GetUserAsync(User);

            var clientenlijst = await _context.Clienten.Where(s => s.HulpverlenerId == user.HulpverlenerId).ToListAsync();
            foreach (var item in clientenlijst)
            {
                apiclients.Add(await ClientApi.PullSpecificClient(item.ClientId));
            }
            ViewData["apiclientslist"] = apiclients;

            return View(new SpecificApiClientHulpverlenerViewModel() { clienten = clientenlijst, specificApiClient = apiclients });
        }

        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> DenyAanmelding(int clientId)
        {
            var aanmelding = _context.Aanmeldingen.Where(s => s.ClientId == clientId).FirstOrDefault();
            var ouder = _context.Ouders.Where(s => s.ClientId == clientId).FirstOrDefault();
            var client = _context.Clienten.Where(s => s.ClientId == clientId).FirstOrDefault();

            var clientUser = _context.Users.Where(s => s.ClientId == clientId).FirstOrDefault();
            var ouderUser = _context.Users.Where(s => s.OuderId == aanmelding.OuderId).FirstOrDefault();

            Task deleteClient = ClientApi.DeleteClient(clientId);
            Task sendClientMail = SendDeniedMail(clientUser);
            Task sendOuderMail;

            _context.Aanmeldingen.Remove(aanmelding);
            _context.Users.Remove(clientUser);
            _context.Users.Remove(ouderUser);
            _context.Clienten.Remove(client);

            if (ouder != null)
            {
                sendOuderMail = SendDeniedMail(ouderUser);
                _context.Ouders.Remove(ouder);
            }

            _context.SaveChanges();

            await deleteClient;
            await sendClientMail;
            await sendClientMail;

            return RedirectToAction("Inbox");
        }
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> AcceptAanmelding(int clientId)
        {
            var aanmelding = _context.Aanmeldingen.Where(s => s.ClientId == clientId).FirstOrDefault();
            var client = _context.Users.Where(s => s.ClientId == clientId).FirstOrDefault();
            var ouder = _context.Users.Where(s => s.OuderId == aanmelding.OuderId).FirstOrDefault();

            var ouderUser = await _usermanager.FindByEmailAsync(ouder.Email);
            var clientUser = await _usermanager.FindByEmailAsync(client.Email);

            ouderUser.EmailConfirmed = true;
            clientUser.EmailConfirmed = true;
            _context.Aanmeldingen.Remove(aanmelding);
            await _context.SaveChangesAsync();

            if (ouder != null)
                await SendAcceptMail(ouderUser);

            await SendAcceptMail(clientUser);

            return RedirectToAction("Inbox");
        }

        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task SendAcceptMail(ApplicationUser user)
        {
            var code = await _usermanager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/RegisterPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await EmailApi.SendMail(new MailRequest()
            {
                ToEmail = user.Email,
                Subject = "ZMDH aanmelding",
                Body = $"U kunt nu een wachtwoord aanmaken via <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>deze link </a>."
            });
        }

        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task SendDeniedMail(ApplicationUser user)
        {
            await EmailApi.SendMail(new MailRequest()
            {
                ToEmail = user.Email,
                Subject = "ZMDH aanmelding",
                Body = $"Uw aanmelding is helaas niet geaccepteerd."
            });
        }

        // GET: Hulpverlener/Details/5
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hulpverlener = await _context.Hulpverleners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hulpverlener == null)
            {
                return NotFound();
            }

            return View(hulpverlener);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> CreateAccount(string Email, string Wachtwoord)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser() { UserName = Email, Email = Email };
                var result = await _usermanager.CreateAsync(user, Wachtwoord);
                var result2 = await _usermanager.AddToRoleAsync(user, "Hulpverlener");
                if (result.Succeeded && result2.Succeeded)
                {
                    TempData["hulpverlenerId"] = user.Id;
                    return RedirectToAction("Create");
                }
            }
            return View("CreateAccount");
        }

        // GET: Hulpverlener/Create
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            ViewBag.newHulpverlenerId = TempData["hulpverlenerId"];
            TempData["i"] = ViewBag.newHulpverlenerId;
            ViewData["Vestigingen"] = _context.Vestigingen.ToList();
            return View();
        }

        // POST: Hulpverlener/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Adres,Specialisatie,Intro,Study,OverJou,Behandeling,Foto,VestigingId")] Hulpverlener hulpverlener)
        {
            ApplicationUser user = _context.Users.Single(s => s.Id == (string)TempData["i"]);
            if (ModelState.IsValid)
            {
                hulpverlener.Account = user;
                _context.Hulpverleners.Add(hulpverlener);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hulpverlener);
        }

        // GET: Hulpverlener/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hulpverlener = await _context.Hulpverleners.FindAsync(id);
            if (hulpverlener == null)
            {
                return NotFound();
            }
            return View(hulpverlener);
        }

        // POST: Hulpverlener/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Adres,Specialisatie,Intro,Study,OverJou,Behandeling,Foto")] Hulpverlener hulpverlener)
        {
            if (id != hulpverlener.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hulpverlener);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HulpverlenerExists(hulpverlener.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hulpverlener);
        }

        // GET: Hulpverlener/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hulpverlener = await _context.Hulpverleners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hulpverlener == null)
            {
                return NotFound();
            }

            return View(hulpverlener);
        }

        // POST: Hulpverlener/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hulpverlener = await _context.Hulpverleners.FindAsync(id);
            _context.Hulpverleners.Remove(hulpverlener);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        private bool HulpverlenerExists(int id)
        {
            return _context.Hulpverleners.Any(e => e.Id == id);
        }
    }
}
