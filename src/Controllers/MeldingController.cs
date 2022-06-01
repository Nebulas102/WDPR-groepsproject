using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace zmdh.Controllers
{
    [Authorize(Roles = "Admin,Hulpverlener")]
    public class MeldingController : Controller
    {
        public readonly DBManager _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public MeldingController(DBManager context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Melding
        public async Task<IActionResult> Index()
        {
            var currentUser = await _usermanager.GetUserAsync(HttpContext.User);
            var currentHulpverlener = await _context.Hulpverleners.Where(h => h.Account.Id == currentUser.Id).SingleOrDefaultAsync();
            return View(await _context.Meldingen.Where(m => m.HulpverlenerId == currentHulpverlener.Id).ToListAsync());
        }
    }
}
