using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GqeberhaClinic.Data;
using GqeberhaClinic.Models;

namespace GqeberhaClinic.Controllers
{
    public class ContraceptivesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContraceptivesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contraceptives
        public async Task<IActionResult> Index()
        {
              return _context.Contraceptive != null ? 
                          View(await _context.Contraceptive.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Contraceptive'  is null.");
        }

        // GET: Contraceptives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contraceptive == null)
            {
                return NotFound();
            }

            var contraceptive = await _context.Contraceptive
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contraceptive == null)
            {
                return NotFound();
            }

            return View(contraceptive);
        }

        // GET: Contraceptives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contraceptives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,DurationOfEffectiveness,MethodOfUse,EfficacyRate,SideEffects,Benefits,Cost,Contraindications,Reversibility,StorageInstructions,AdditionalNotes")] Contraceptive contraceptive)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contraceptive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contraceptive);
        }

        // GET: Contraceptives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contraceptive == null)
            {
                return NotFound();
            }

            var contraceptive = await _context.Contraceptive.FindAsync(id);
            if (contraceptive == null)
            {
                return NotFound();
            }
            return View(contraceptive);
        }

        // POST: Contraceptives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,DurationOfEffectiveness,MethodOfUse,EfficacyRate,SideEffects,Benefits,Cost,Contraindications,Reversibility,StorageInstructions,AdditionalNotes")] Contraceptive contraceptive)
        {
            if (id != contraceptive.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contraceptive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContraceptiveExists(contraceptive.Id))
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
            return View(contraceptive);
        }

        // GET: Contraceptives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contraceptive == null)
            {
                return NotFound();
            }

            var contraceptive = await _context.Contraceptive
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contraceptive == null)
            {
                return NotFound();
            }

            return View(contraceptive);
        }

        // POST: Contraceptives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contraceptive == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contraceptive'  is null.");
            }
            var contraceptive = await _context.Contraceptive.FindAsync(id);
            if (contraceptive != null)
            {
                _context.Contraceptive.Remove(contraceptive);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContraceptiveExists(int id)
        {
          return (_context.Contraceptive?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
