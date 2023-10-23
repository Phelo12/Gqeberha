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
    public class ReportCasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportCasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReportCases
        public async Task<IActionResult> Index()
        {
              return _context.ReportCases != null ? 
                          View(await _context.ReportCases.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ReportCases'  is null.");
        }

        // GET: ReportCases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReportCases == null)
            {
                return NotFound();
            }

            var reportCase = await _context.ReportCases
                .FirstOrDefaultAsync(m => m.caseID == id);
            if (reportCase == null)
            {
                return NotFound();
            }

            return View(reportCase);
        }

        // GET: ReportCases/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReportCases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("caseID,FullName,Address,Communication,Time,caseDescription")] ReportCase reportCase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reportCase);
        }

        // GET: ReportCases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReportCases == null)
            {
                return NotFound();
            }

            var reportCase = await _context.ReportCases.FindAsync(id);
            if (reportCase == null)
            {
                return NotFound();
            }
            return View(reportCase);
        }

        // POST: ReportCases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("caseID,FullName,Address,Communication,Time,caseDescription")] ReportCase reportCase)
        {
            if (id != reportCase.caseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportCaseExists(reportCase.caseID))
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
            return View(reportCase);
        }

        // GET: ReportCases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReportCases == null)
            {
                return NotFound();
            }

            var reportCase = await _context.ReportCases
                .FirstOrDefaultAsync(m => m.caseID == id);
            if (reportCase == null)
            {
                return NotFound();
            }

            return View(reportCase);
        }

        // POST: ReportCases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReportCases == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ReportCases'  is null.");
            }
            var reportCase = await _context.ReportCases.FindAsync(id);
            if (reportCase != null)
            {
                _context.ReportCases.Remove(reportCase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportCaseExists(int id)
        {
          return (_context.ReportCases?.Any(e => e.caseID == id)).GetValueOrDefault();
        }
    }
}
