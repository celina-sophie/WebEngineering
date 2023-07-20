using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEngineering.Data;
using WebEngineering.Models;

namespace WebEngineering.Controllers
{
    [Authorize]
    public class LieferungController : Controller
    {
        private readonly IdentityContext _context;

        public LieferungController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Lieferung
        public async Task<IActionResult> Index()
        {
            var identityContext = _context.Lieferungen.Include(l => l.Produkt);
            return View(await identityContext.ToListAsync());
        }

        // GET: Lieferung/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lieferungen == null)
            {
                return NotFound();
            }

            var lieferung = await _context.Lieferungen
                .Include(l => l.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lieferung == null)
            {
                return NotFound();
            }

            return View(lieferung);
        }

        // GET: Lieferung/Create
        public IActionResult Create()
        {
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Name");
            return View();
        }

        // POST: Lieferung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProduktId,Date,Menge")] Lieferung lieferung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lieferung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Id", lieferung.ProduktId);
            return View(lieferung);
        }

        // GET: Lieferung/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lieferungen == null)
            {
                return NotFound();
            }

            var lieferung = await _context.Lieferungen.FindAsync(id);
            if (lieferung == null)
            {
                return NotFound();
            }
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Id", lieferung.ProduktId);
            return View(lieferung);
        }

        // POST: Lieferung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProduktId,Date,Menge")] Lieferung lieferung)
        {
            if (id != lieferung.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lieferung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LieferungExists(lieferung.Id))
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
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Id", lieferung.ProduktId);
            return View(lieferung);
        }

        // GET: Lieferung/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lieferungen == null)
            {
                return NotFound();
            }

            var lieferung = await _context.Lieferungen
                .Include(l => l.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lieferung == null)
            {
                return NotFound();
            }

            return View(lieferung);
        }

        // POST: Lieferung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lieferungen == null)
            {
                return Problem("Entity set 'IdentityContext.Lieferungen'  is null.");
            }
            var lieferung = await _context.Lieferungen.FindAsync(id);
            if (lieferung != null)
            {
                _context.Lieferungen.Remove(lieferung);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LieferungExists(int id)
        {
          return (_context.Lieferungen?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
