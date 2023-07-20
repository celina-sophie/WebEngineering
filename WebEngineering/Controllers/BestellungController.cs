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
    public class BestellungController : Controller
    {
        private readonly IdentityContext _context;

        public BestellungController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Bestellung
        public async Task<IActionResult> Index()
        {
            var identityContext = _context.Bestellungen.Include(b => b.Produkt);
            return View(await identityContext.ToListAsync());
        }

        // GET: Bestellung/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bestellungen == null)
            {
                return NotFound();
            }

            var bestellung = await _context.Bestellungen
                .Include(b => b.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestellung == null)
            {
                return NotFound();
            }

            return View(bestellung);
        }

        // GET: Bestellung/Create
        public IActionResult Create()
        {
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Id");
            return View();
        }

        // POST: Bestellung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProduktId,Date,Menge")] Bestellung bestellung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bestellung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    var errorMessage = error.ErrorMessage;
                    Console.WriteLine(errorMessage);
                }
            }
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Id", bestellung.ProduktId);
            return View(bestellung);
        }

        // GET: Bestellung/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bestellungen == null)
            {
                return NotFound();
            }

            var bestellung = await _context.Bestellungen.FindAsync(id);
            if (bestellung == null)
            {
                return NotFound();
            }
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Id", bestellung.ProduktId);
            return View(bestellung);
        }

        // POST: Bestellung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProduktId,Date,Menge")] Bestellung bestellung)
        {
            if (id != bestellung.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestellung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellungExists(bestellung.Id))
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
            ViewData["ProduktId"] = new SelectList(_context.Produkte, "Id", "Id", bestellung.ProduktId);
            return View(bestellung);
        }

        // GET: Bestellung/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bestellungen == null)
            {
                return NotFound();
            }

            var bestellung = await _context.Bestellungen
                .Include(b => b.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestellung == null)
            {
                return NotFound();
            }

            return View(bestellung);
        }

        // POST: Bestellung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bestellungen == null)
            {
                return Problem("Entity set 'IdentityContext.Bestellungen'  is null.");
            }
            var bestellung = await _context.Bestellungen.FindAsync(id);
            if (bestellung != null)
            {
                _context.Bestellungen.Remove(bestellung);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellungExists(int id)
        {
          return (_context.Bestellungen?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
