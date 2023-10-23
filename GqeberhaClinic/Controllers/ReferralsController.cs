using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GqeberhaClinic.Data;
using GqeberhaClinic.Models;
using System.Security.Claims;

namespace GqeberhaClinic.Controllers
{
    public class ReferralsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReferralsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Referrals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Referrals.Include(r => r.ContraceptivePlan).Include(r => r.ContraceptivePlan.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Referrals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Referrals == null)
            {
                return NotFound();
            }

            var referrals = await _context.Referrals
                .Include(r => r.ContraceptivePlan)
                .FirstOrDefaultAsync(m => m.ReferralId == id);
            if (referrals == null)
            {
                return NotFound();
            }

            return View(referrals);
        }

        // GET: Referrals/Create
        public IActionResult Create(int? Id)
        {
            if(Id != null)
            {
                ViewBag.Id = Id;
            }
            ViewData["PreventionID"] = new SelectList(_context.ContraceptivePrescription, "PrescriptionId", "ContraceptiveName");
            return View();
        }

        // POST: Referrals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReferralId,NurseID,ReferralDate,ReasonForReferral,ReferredTo,ReferringDoctor,Notes,PreventionID")] Referrals referrals)
        {
            if (ModelState.IsValid)
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                referrals.NurseID = user;
                _context.Add(referrals);
                await _context.SaveChangesAsync();
                var alert = new Alert()
                {
                    Message = "Screening has been done sucessfully",
                    IntendedUser = user,
                    Role = "notification",

                };
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PreventionID"] = new SelectList(_context.ContraceptivePrescription, "PrescriptionId", "ContraceptiveName", referrals.PreventionID);
            return View(referrals);
        }

        // GET: Referrals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Referrals == null)
            {
                return NotFound();
            }

            var referrals = await _context.Referrals.FindAsync(id);
            if (referrals == null)
            {
                return NotFound();
            }
            ViewData["PreventionID"] = new SelectList(_context.ContraceptivePrescription, "PrescriptionId", "ContraceptiveName", referrals.PreventionID);
            return View(referrals);
        }

        // POST: Referrals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReferralId,NurseID,ReferralDate,ReasonForReferral,ReferredTo,ReferringDoctor,Notes,PreventionID")] Referrals referrals)
        {
            if (id != referrals.ReferralId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(referrals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReferralsExists(referrals.ReferralId))
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
            ViewData["PreventionID"] = new SelectList(_context.ContraceptivePrescription, "PrescriptionId", "ContraceptiveName", referrals.PreventionID);
            return View(referrals);
        }

        // GET: Referrals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Referrals == null)
            {
                return NotFound();
            }

            var referrals = await _context.Referrals
                .Include(r => r.ContraceptivePlan)
                .FirstOrDefaultAsync(m => m.ReferralId == id);
            if (referrals == null)
            {
                return NotFound();
            }

            return View(referrals);
        }

        // POST: Referrals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Referrals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Referrals'  is null.");
            }
            var referrals = await _context.Referrals.FindAsync(id);
            if (referrals != null)
            {
                _context.Referrals.Remove(referrals);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReferralsExists(int id)
        {
          return (_context.Referrals?.Any(e => e.ReferralId == id)).GetValueOrDefault();
        }
    }
}
