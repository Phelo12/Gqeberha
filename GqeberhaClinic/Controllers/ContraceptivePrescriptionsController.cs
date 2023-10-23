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
    public class ContraceptivePrescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContraceptivePrescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContraceptivePrescriptions
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.His = _context.ContraceptivePrescription.Include(c => c.Patient).ToList();
            ViewBag.Pa = (from U in _context.Users
                          join UR in _context.UserRoles on U.Id equals UR.UserId
                          join R in _context.Roles on UR.RoleId equals R.Id
                          where R.Name == "Patient"
                          select U).ToList();
            return View();
        }   
        public async Task<IActionResult> My_Plan_History()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.His = _context.ContraceptivePrescription.Include(c => c.Patient).Where(a => a.PatientId == user).ToList();
            ViewBag.Pa = (from U in _context.Users
                          join UR in _context.UserRoles on U.Id equals UR.UserId
                          join R in _context.Roles on UR.RoleId equals R.Id
                          where R.Name == "Patient"
                          select U).ToList();
            return View();
        }

        // GET: ContraceptivePrescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContraceptivePrescription == null)
            {
                return NotFound();
            }

            var contraceptivePrescription = await _context.ContraceptivePrescription
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.PrescriptionId == id);
            if (contraceptivePrescription == null)
            {
                return NotFound();
            }

            return View(contraceptivePrescription);
        }

        // GET: ContraceptivePrescriptions/Create
        public IActionResult Create()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ContraceptivePrescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrescriptionId,PatientId,ContraceptiveName,PrescribedDate,StartDate,EndDate,Dosage,Frequency,Notes")] ContraceptivePrescription contraceptivePrescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contraceptivePrescription);
                await _context.SaveChangesAsync();
                var alert = new Alert()
                {
                    IntendedUser = contraceptivePrescription.PatientId,
                    Message = "Contraceptive Plan have been created for you by our Nurse",
                    Role = "Notification"
                };
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New Contrceptive Plan has be created successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", contraceptivePrescription.PatientId);
            return View(contraceptivePrescription);
        }

        // GET: ContraceptivePrescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContraceptivePrescription == null)
            {
                return NotFound();
            }

            var contraceptivePrescription = await _context.ContraceptivePrescription.FindAsync(id);
            if (contraceptivePrescription == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", contraceptivePrescription.PatientId);
            return View(contraceptivePrescription);
        }

        // POST: ContraceptivePrescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrescriptionId,PatientId,ContraceptiveName,PrescribedDate,StartDate,EndDate,Dosage,Frequency,Notes")] ContraceptivePrescription contraceptivePrescription)
        {
            if (id != contraceptivePrescription.PrescriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contraceptivePrescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContraceptivePrescriptionExists(contraceptivePrescription.PrescriptionId))
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
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", contraceptivePrescription.PatientId);
            return View(contraceptivePrescription);
        }

        // GET: ContraceptivePrescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContraceptivePrescription == null)
            {
                return NotFound();
            }

            var contraceptivePrescription = await _context.ContraceptivePrescription
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.PrescriptionId == id);
            if (contraceptivePrescription == null)
            {
                return NotFound();
            }

            return View(contraceptivePrescription);
        }

        // POST: ContraceptivePrescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContraceptivePrescription == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ContraceptivePrescription'  is null.");
            }
            var contraceptivePrescription = await _context.ContraceptivePrescription.FindAsync(id);
            if (contraceptivePrescription != null)
            {
                _context.ContraceptivePrescription.Remove(contraceptivePrescription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContraceptivePrescriptionExists(int id)
        {
          return (_context.ContraceptivePrescription?.Any(e => e.PrescriptionId == id)).GetValueOrDefault();
        }
    }
}
