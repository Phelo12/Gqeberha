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
    public class Appointments_QuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Appointments_QuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments_Ques
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
                ViewBag.Alert = alert;
                var applicationDbContext = _context.Appointments_Ques.Include(a => a.Appointments).Include(a => a.Appointments.Patient).OrderByDescending(a => a.dateOFQue);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
                ViewBag.Alert = alert;
                var applicationDbContext = _context.Appointments_Ques.Include(a => a.Appointments).Include(a => a.Appointments.Patient).Where(a => a.AppointmentID == id).OrderByDescending(a => a.dateOFQue);
                return View(await applicationDbContext.ToListAsync());
            }
         
        }

        // GET: Appointments_Ques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments_Ques == null)
            {
                return NotFound();
            }

            var appointments_Ques = await _context.Appointments_Ques
                .Include(a => a.Appointments)
    
                .FirstOrDefaultAsync(m => m.QueID == id);
            if (appointments_Ques == null)
            {
                return NotFound();
            }

            return View(appointments_Ques);
        }

        // GET: Appointments_Ques/Create
        public IActionResult Create()
        {
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "Reason");
            ViewData["ClinicianID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Appointments_Ques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Extend(Appointment_QueVM appointments_Ques)
        {
            if (appointments_Ques.RoomNumber !=null && appointments_Ques.ClinicianID != null)
            {

                var Ques = _context.Appointments_Ques.Where(a => a.AppointmentID == appointments_Ques.AppointmentID).FirstOrDefault();
                Ques.Time = appointments_Ques.Time;
                _context.Appointments_Ques.Update(Ques);
                await _context.SaveChangesAsync();
                var appoint = _context.Appointments.Where(a => a.AppointmentID == appointments_Ques.AppointmentID).FirstOrDefault();
                appoint.Status = "Delay Extended";
                _context.Appointments.Update(appoint);
                await _context.SaveChangesAsync();

                var alert = new Alert()
                {
                    IntendedUser = appoint.PatientID,
                    Message = "Please Note your Appointment delay has been extend with " + appointments_Ques.Time+" Apology for the inconvinience",
                    Role = "Notification"
                };
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "Reason", appointments_Ques.AppointmentID);
            ViewData["ClinicianID"] = new SelectList(_context.Users, "Id", "Id", appointments_Ques.ClinicianID);
            return View(appointments_Ques);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create1(Appointment_QueVM appointments_Ques)
        {
            if (appointments_Ques.RoomNumber !=null && appointments_Ques.ClinicianID != null)
            {
                var Ques = new Appointments_Ques()
                {
                    RoomNumber = appointments_Ques.RoomNumber,
                    Clinician = appointments_Ques.ClinicianID,
                    AppointmentID = appointments_Ques.AppointmentID,

                    Time = appointments_Ques.Time
                };

                _context.Appointments_Ques.Add(Ques);
                await _context.SaveChangesAsync();
                var appoint = _context.Appointments.Where(a => a.AppointmentID == appointments_Ques.AppointmentID).FirstOrDefault();
                appoint.Status = Ques.Status;
                _context.Appointments.Update(appoint);
                await _context.SaveChangesAsync();

                var alert = new Alert()
                {
                    IntendedUser = appoint.PatientID,
                    Message = "Please Note you been placed on a Que for " + appointments_Ques.Time,
                    Role = "Notification"
                };
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "Reason", appointments_Ques.AppointmentID);
            ViewData["ClinicianID"] = new SelectList(_context.Users, "Id", "Id", appointments_Ques.ClinicianID);
            return View(appointments_Ques);
        }

        // GET: Appointments_Ques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments_Ques == null)
            {
                return NotFound();
            }

            var appointments_Ques = await _context.Appointments_Ques.FindAsync(id);
            if (appointments_Ques == null)
            {
                return NotFound();
            }
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "Reason", appointments_Ques.AppointmentID);
            ViewData["ClinicianID"] = new SelectList(_context.Users, "Id", "Id", appointments_Ques.Clinician);
            return View(appointments_Ques);
        }

        // POST: Appointments_Ques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QueID,AppointmentID,dateOFQue,Time,RoomNumber,ClinicianID,Status")] Appointments_Ques appointments_Ques)
        {
            if (id != appointments_Ques.QueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointments_Ques);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Appointments_QuesExists(appointments_Ques.QueID))
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
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "Reason", appointments_Ques.AppointmentID);
            ViewData["ClinicianID"] = new SelectList(_context.Users, "Id", "Id", appointments_Ques.Clinician);
            return View(appointments_Ques);
        }

        // GET: Appointments_Ques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments_Ques == null)
            {
                return NotFound();
            }

            var appointments_Ques = await _context.Appointments_Ques
                .Include(a => a.Appointments)
                .FirstOrDefaultAsync(m => m.QueID == id);
            if (appointments_Ques == null)
            {
                return NotFound();
            }

            return View(appointments_Ques);
        }

        // POST: Appointments_Ques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments_Ques == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointments_Ques'  is null.");
            }
            var appointments_Ques = await _context.Appointments_Ques.FindAsync(id);
            if (appointments_Ques != null)
            {
                _context.Appointments_Ques.Remove(appointments_Ques);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Appointments_QuesExists(int id)
        {
          return (_context.Appointments_Ques?.Any(e => e.QueID == id)).GetValueOrDefault();
        }
    }
}
