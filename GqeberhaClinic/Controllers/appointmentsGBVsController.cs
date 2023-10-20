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
    public class appointmentsGBVsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public appointmentsGBVsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: appointmentsGBVs
        public async Task<IActionResult> Index()
        {
              return _context.appointmentsGBVs != null ? 
                          View(await _context.appointmentsGBVs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.appointmentsGBVs'  is null.");
        }

        // GET: appointmentsGBVs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.appointmentsGBVs == null)
            {
                return NotFound();
            }

            var appointmentsGBV = await _context.appointmentsGBVs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentsGBV == null)
            {
                return NotFound();
            }

            return View(appointmentsGBV);
        }

        // GET: appointmentsGBVs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: appointmentsGBVs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Purpose,Date")] appointmentsGBV appointmentsGBV)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentsGBV);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentsGBV);
        }

        // GET: appointmentsGBVs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.appointmentsGBVs == null)
            {
                return NotFound();
            }

            var appointmentsGBV = await _context.appointmentsGBVs.FindAsync(id);
            if (appointmentsGBV == null)
            {
                return NotFound();
            }
            return View(appointmentsGBV);
        }

        // POST: appointmentsGBVs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Purpose,Date")] appointmentsGBV appointmentsGBV)
        {
            if (id != appointmentsGBV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentsGBV);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!appointmentsGBVExists(appointmentsGBV.Id))
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
            return View(appointmentsGBV);
        }

        // GET: appointmentsGBVs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.appointmentsGBVs == null)
            {
                return NotFound();
            }

            var appointmentsGBV = await _context.appointmentsGBVs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentsGBV == null)
            {
                return NotFound();
            }

            return View(appointmentsGBV);
        }

        // POST: appointmentsGBVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.appointmentsGBVs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.appointmentsGBVs'  is null.");
            }
            var appointmentsGBV = await _context.appointmentsGBVs.FindAsync(id);
            if (appointmentsGBV != null)
            {
                _context.appointmentsGBVs.Remove(appointmentsGBV);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool appointmentsGBVExists(int id)
        {
          return (_context.appointmentsGBVs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
