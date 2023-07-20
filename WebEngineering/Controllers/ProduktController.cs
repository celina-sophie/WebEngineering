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
    public class ProduktController : Controller
    {
        private readonly IdentityContext _context;

        public ProduktController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Produkt
        public async Task<IActionResult> Index()
        {
              return _context.Produkte != null ? 
                          View(await _context.Produkte.ToListAsync()) :
                          Problem("Entity set 'IdentityContext.Produkte'  is null.");
        }


        // Inventory
        // Inventory
        public async Task<IActionResult> InventoryHistory(int produktId)
        {
            if (produktId <= 0)
            {
                return BadRequest("Ungültige Produkt-ID.");
            }

            var lieferungen = await _context.Lieferungen
                .Where(l => l.ProduktId == produktId)
                .ToListAsync();

            var bestellungen = await _context.Bestellungen
                .Where(b => b.ProduktId == produktId)
                .ToListAsync();

            var inventoryHistory = new List<Inventory>();

            int currentInventory = 0;

            foreach (var lieferung in lieferungen)
            {
                currentInventory += lieferung.Menge;
                inventoryHistory.Add(new Inventory
                {
                    Datum = lieferung.Date,
                    Lagerbestand = currentInventory
                });
            }

            foreach (var bestellung in bestellungen)
            {
                currentInventory -= bestellung.Menge;
                inventoryHistory.Add(new Inventory
                {
                    Datum = bestellung.Date,
                    Lagerbestand = currentInventory
                });
            }

            ViewBag.ProduktId = produktId;
            inventoryHistory = inventoryHistory.OrderBy(i => i.Datum).ToList();
            ViewBag.InventoryHistory = inventoryHistory;

            return View();
        }


        // Average
        public async Task<IActionResult> DurchschnittlicheMengen()
        {
            var produkte = await _context.Produkte
        .Include(p => p.Bestellungen)
        .Include(p => p.Lieferungen)
        .ToListAsync();

            var durchschnittlicheMengen = new List<Average>();

            foreach (var produkt in produkte)
            {

                var durchschnittlicheBestellmenge = produkt.Bestellungen.Any()==true
                    ? produkt.Bestellungen.Average(b => b.Menge)
                    : 0;

                var durchschnittlicheLiefermenge = produkt.Lieferungen.Any()== true
                    ? 
                     produkt.Lieferungen.Average(l => l.Menge):0;

                durchschnittlicheMengen.Add(new Average
                {
                    Id = produkt.Id,
                    Name = produkt.Name,
                    DurchschnittlicheBestellmenge = (decimal)durchschnittlicheBestellmenge,
                    DurchschnittlicheLiefermenge = (decimal)durchschnittlicheLiefermenge,
                });
            }

            ViewBag.Produkte = durchschnittlicheMengen;

            return View();
        }

        // GET: Produkt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produkte == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkte
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // GET: Produkt/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produkt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Produkt produkt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produkt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produkt);
        }

        // GET: Produkt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produkte == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkte.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }
            return View(produkt);
        }

        // POST: Produkt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Produkt produkt)
        {
            if (id != produkt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produkt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktExists(produkt.Id))
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
            return View(produkt);
        }

        // GET: Produkt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produkte == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkte
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Produkt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produkte == null)
            {
                return Problem("Entity set 'IdentityContext.Produkte'  is null.");
            }
            var produkt = await _context.Produkte.FindAsync(id);
            if (produkt != null)
            {
                _context.Produkte.Remove(produkt);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktExists(int id)
        {
          return (_context.Produkte?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
