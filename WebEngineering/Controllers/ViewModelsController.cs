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
    public class ViewModelsController : Controller
    {
        private readonly IdentityContext _context;

        public ViewModelsController(IdentityContext context)
        {
            _context = context;
        }

        // GET: ViewModels
        public async Task<IActionResult> Index()
        {
            var produkt = _context.Produkte.FirstOrDefault();
            var bestellung = _context.Bestellungen.FirstOrDefault();
            var lieferung = _context.Lieferungen.FirstOrDefault();

            var viewModel = new ViewModel
            {
                Produkt = produkt,
                Bestellung = bestellung,
                Lieferung = lieferung
            };

            return View(viewModel);
        }

        // GET: ViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ViewModel == null)
            {
                return NotFound();
            }

            var viewModel = await _context.ViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: ViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] ViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: ViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ViewModel == null)
            {
                return NotFound();
            }

            var viewModel = await _context.ViewModel.FindAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: ViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] ViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViewModelExists(viewModel.Id))
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
            return View(viewModel);
        }

        // GET: ViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ViewModel == null)
            {
                return NotFound();
            }

            var viewModel = await _context.ViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: ViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ViewModel == null)
            {
                return Problem("Entity set 'IdentityContext.ViewModel'  is null.");
            }
            var viewModel = await _context.ViewModel.FindAsync(id);
            if (viewModel != null)
            {
                _context.ViewModel.Remove(viewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViewModelExists(int id)
        {
          return (_context.ViewModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
