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
    public class Vaccine_EducationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Vaccine_EducationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vaccine_Education
        public async Task<IActionResult> Index()
        {
              return _context.Vaccine_Education != null ? 
                          View(await _context.Vaccine_Education.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Vaccine_Education'  is null.");
        }

        // GET: Vaccine_Education/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vaccine_Education == null)
            {
                return NotFound();
            }

            var vaccine_Education = await _context.Vaccine_Education
                .FirstOrDefaultAsync(m => m.EducationID == id);
            if (vaccine_Education == null)
            {
                return NotFound();
            }

            return View(vaccine_Education);
        }

        // GET: Vaccine_Education/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vaccine_Education/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vaccine_Education vaccine_Education, IFormFile imagefile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(imagefile  != null && imagefile.Length > 0)
                    {
                        using ( var memorystream = new MemoryStream())
                        {
                            await imagefile.CopyToAsync(memorystream);

                            vaccine_Education.Image = memorystream.ToArray();
                        }
                    }
                }
                catch
                {

                }
                _context.Add(vaccine_Education);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vaccine_Education);
        }

        // GET: Vaccine_Education/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vaccine_Education == null)
            {
                return NotFound();
            }

            var vaccine_Education = await _context.Vaccine_Education.FindAsync(id);
            if (vaccine_Education == null)
            {
                return NotFound();
            }
            return View(vaccine_Education);
        }

        // POST: Vaccine_Education/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EducationID,VaccineName,VaccineDescription,Purpose,Image")] Vaccine_Education vaccine_Education)
        {
            if (id != vaccine_Education.EducationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccine_Education);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Vaccine_EducationExists(vaccine_Education.EducationID))
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
            return View(vaccine_Education);
        }

        // GET: Vaccine_Education/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vaccine_Education == null)
            {
                return NotFound();
            }

            var vaccine_Education = await _context.Vaccine_Education
                .FirstOrDefaultAsync(m => m.EducationID == id);
            if (vaccine_Education == null)
            {
                return NotFound();
            }

            return View(vaccine_Education);
        }

        // POST: Vaccine_Education/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vaccine_Education == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vaccine_Education'  is null.");
            }
            var vaccine_Education = await _context.Vaccine_Education.FindAsync(id);
            if (vaccine_Education != null)
            {
                _context.Vaccine_Education.Remove(vaccine_Education);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Vaccine_EducationExists(int id)
        {
          return (_context.Vaccine_Education?.Any(e => e.EducationID == id)).GetValueOrDefault();
        }
    }
}
