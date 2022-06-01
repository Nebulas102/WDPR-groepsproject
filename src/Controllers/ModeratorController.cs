using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static zmdh.Areas.Identity.Pages.Account.RegisterModel;

namespace zmdh.Controllers
{
    public class ModeratorController : Controller
    {
        private readonly DBManager _context;

        public ModeratorController(DBManager context)
        {
            _context = context;
        }

        // GET: Moderator
        
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Moderator.ToListAsync());
        }

        // GET: Moderator/Details/5
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moderator = await _context.Moderator
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moderator == null)
            {
                return NotFound();
            }

            return View(moderator);
        }

        // GET: Moderator/Create
        [Authorize(Roles="Moderator")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> HulpverlenerClientlist()
        {
            List<HulpverlenerClientRelationViewModel> model = await fillList();
            return View(model);
        }
        [Authorize(Roles="Moderator")]
        private async Task<List<HulpverlenerClientRelationViewModel>> fillList()
        {
            List<HulpverlenerClientRelationViewModel> model = new List<HulpverlenerClientRelationViewModel>();
            List<Client> clientList =  _context.Clienten.Include(s => s.Hulpverlener).ToList();
            foreach(var item in clientList)
            {
                HulpverlenerClientRelationViewModel viewModel = new HulpverlenerClientRelationViewModel();
                SpecificApiClient client = await ClientApi.PullSpecificClient(item.ClientId);
                viewModel.naamClient = client.volledigenaam;
                viewModel.naamHulpverlener = item.Hulpverlener.Name;
                viewModel.specialisatieHulpverlener = item.Hulpverlener.Specialisatie;
                model.Add(viewModel);
            }
            model.OrderBy(p => p.naamHulpverlener);
            return model;
        }

        // POST: Moderator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> Create([Bind("Id")] Moderator moderator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moderator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moderator);
        }

        // GET: Moderator/Edit/5
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moderator = await _context.Moderator.FindAsync(id);
            if (moderator == null)
            {
                return NotFound();
            }
            return View(moderator);
        }

        // POST: Moderator/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Moderator moderator)
        {
            if (id != moderator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moderator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeratorExists(moderator.Id))
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
            return View(moderator);
        }

        // GET: Moderator/Delete/5
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moderator = await _context.Moderator
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moderator == null)
            {
                return NotFound();
            }

            return View(moderator);
        }

        // POST: Moderator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moderator = await _context.Moderator.FindAsync(id);
            _context.Moderator.Remove(moderator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles="Moderator")]
        private bool ModeratorExists(int id)
        {
            return _context.Moderator.Any(e => e.Id == id);
        }
    }
}
