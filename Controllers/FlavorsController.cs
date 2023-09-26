using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CloudComputingProject.Data;
using CloudComputingProject.Models;

namespace CloudComputingProject.Controllers
{
    public class FlavorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlavorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flavors
        public async Task<IActionResult> Index()
        {
              return _context.Flavors != null ? 
                          View(await _context.Flavors.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Flavors'  is null.");
        }

        // GET: Flavors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Flavors == null)
            {
                return NotFound();
            }

            var flavor = await _context.Flavors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flavor == null)
            {
                return NotFound();
            }

            return View(flavor);
        }

        // GET: Flavors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flavors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlavorName,Categoty,IsAvailable,FlavorUrl")] Flavor flavor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flavor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flavor);
        }

        // GET: Flavors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Flavors == null)
            {
                return NotFound();
            }

            var flavor = await _context.Flavors.FindAsync(id);
            if (flavor == null)
            {
                return NotFound();
            }
            return View(flavor);
        }

        // POST: Flavors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlavorName,Categoty,IsAvailable,FlavorUrl")] Flavor flavor)
        {
            if (id != flavor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flavor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlavorExists(flavor.Id))
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
            return View(flavor);
        }

        // GET: Flavors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Flavors == null)
            {
                return NotFound();
            }

            var flavor = await _context.Flavors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flavor == null)
            {
                return NotFound();
            }

            return View(flavor);
        }

        // POST: Flavors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Flavors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Flavors'  is null.");
            }
            var flavor = await _context.Flavors.FindAsync(id);
            if (flavor != null)
            {
                _context.Flavors.Remove(flavor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlavorExists(int id)
        {
          return (_context.Flavors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
