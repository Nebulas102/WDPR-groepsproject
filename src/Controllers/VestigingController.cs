using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace zmdh.Controllers
{
    [Authorize(Roles="Moderator")]
    public class VestigingController : Controller
    {
        public readonly DBManager _context;

        public VestigingController(DBManager context)
        {
            _context = context;
        }

        // GET: Vestiging
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vestigingen.ToListAsync());
        }

        // GET: Vestiging/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vestiging = await _context.Vestigingen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vestiging == null)
            {
                return NotFound();
            }

            return View(vestiging);
        }

        // GET: Vestiging/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vestiging/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Adress,Plaats")] Vestiging vestiging)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vestiging);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vestiging);
        }

        // GET: Vestiging/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vestiging = await _context.Vestigingen.FindAsync(id);
            if (vestiging == null)
            {
                return NotFound();
            }
            return View(vestiging);
        }

        // POST: Vestiging/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Adress,Plaats")] Vestiging vestiging)
        {
            if (id != vestiging.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vestiging);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VestigingExists(vestiging.Id))
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
            return View(vestiging);
        }

        // GET: Vestiging/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vestiging = await _context.Vestigingen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vestiging == null)
            {
                return NotFound();
            }

            return View(vestiging);
        }

        // POST: Vestiging/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vestiging = await _context.Vestigingen.FindAsync(id);
            _context.Vestigingen.Remove(vestiging);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VestigingExists(int id)
        {
            return _context.Vestigingen.Any(e => e.Id == id);
        }
    }
}
