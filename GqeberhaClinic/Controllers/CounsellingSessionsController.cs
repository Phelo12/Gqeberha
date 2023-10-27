using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using GqeberhaClinic.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using GqeberhaClinic.Data;

namespace GqeberhaClinic.Controllers
{
    public class CounsellingSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender emailSender;
        public CounsellingSessionsController(ApplicationDbContext context, IEmailSender _emailSender)
        {
            _context = context;
            emailSender = _emailSender;
        }

        //Get: Counselling_Sessions
        public async Task<IActionResult> My_Sessions()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a =>a.Date).ToList();
            if(Alert.Count > 0)
            {
                ViewBag.Alert = Alert;
                TempData["Alerts"] = "Not null";

            }
            ViewBag.Sessions = await _context.Counselling_Sessions.Include(c => c.Appointment).Include(c => c.Counsellor).Include(a => a.Appointment.Patient).Where(a => a.Appointment.PatientID == user).OrderByDescending(a => a.SessionCreatedDate).ToListAsync();
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            if (Alert.Count > 0)
            {
                ViewBag.Alert = Alert;
                TempData["Alerts"] = "Not null";
            }
            ViewBag.Sessions = await _context.Counselling_Sessions.Include(c => c.Appointment).Include(c => c.Counsellor).Include(a => a.Appointment.Patient).OrderByDescending(a => a.SessionCreatedDate).ToListAsync();
            return View();
        }
        public async Task<IActionResult> Report()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Date = DateTime.Now.ToString("dd/MMMM/yyyy");
            ViewBag.Time = DateTime.Now.ToString("HH:MM");
            var Alerts = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            if (Alerts.Count > 0)
            {
                ViewBag.Alerts = Alerts;
                TempData["Alerts"] = "Not null";
            }
            ViewBag.Sessions = await _context.Counselling_Sessions.Include(c => c.Appointment).Include(c => c.Counsellor).Include(a => a.Appointment.Patient).ToListAsync();
            return View();
        }
        public async Task<IActionResult> My_Report()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Date = DateTime.Now.ToString("dd/MMMM/yyyy");
            ViewBag.Time = DateTime.Now.ToString("HH:MM");
            var Alerts = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            if (Alerts.Count > 0)
            {
                ViewBag.Alerts = Alerts;
                TempData["Alerts"] = "Not null";
            }
            ViewBag.Sessions = await _context.Counselling_Sessions.Include(c => c.Appointment).Include(c => c.Counsellor).Include(a => a.Appointment.Patient).Where(a => a.Appointment.Patient.Id == user).ToListAsync();
            return View();
        }
        //GET: Counselling_Sessions/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Counselling_Sessions == null)
            {
                return NotFound();
            }

            var counselling_Sessions = await _context.Counselling_Sessions
                .Include(c => c.Appointment)
                .Include(c => c.Counsellor)
                .FirstOrDefaultAsync(m => m.SessionID == id);
            if (counselling_Sessions == null)
            {
                return NotFound();
            }

            return View(counselling_Sessions);
        }
        //GET: Counselling_Sessions/Create
        public IActionResult Create()
        {
            var appointments = _context.Appointments.ToList();
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "Reason");
            ViewData["Appointments"] = new SelectList(appointments, "AppointmentID", "Reason");

            // Get the currently logged-in user's ID
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Set the CounsellorID to the logged-in user's ID
            ViewData["CounsellorID"] = new SelectList(_context.Users.Where(u => u.Id == user), "Id", "Id");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Counselling_Sessions counselling_Sessions)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            counselling_Sessions.CounsellorID = user;
            try
            {
                // The rest of your code for creating a session
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentId", "Reason", counselling_Sessions.AppointmentID);

            // Populate CounsellorID based on the logged-in user
            ViewData["CounsellorID"] = new SelectList(_context.Users.Where(u => u.Id == user), "Id", "Id", counselling_Sessions.CounsellorID);

            return View(counselling_Sessions);
        }

        public async Task<IActionResult> Close_Session(int? id)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null || _context.Counselling_Sessions == null)
            {
                return NotFound();
            }
            var counselling_Sessions = await _context.Counselling_Sessions.FindAsync(id);
            if (counselling_Sessions == null)
            {
                return NotFound();
            }
            counselling_Sessions.Status = "Session Closed";
            _context.Update(counselling_Sessions);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Session has been Closed Successfully";
            //Create alert for Patient
            var p = _context.Appointments.Where(a => a.AppointmentID == counselling_Sessions.AppointmentID).FirstOrDefault();
            var alert = new Alert()
            {
                Message = "Counselling Session, dated on " + counselling_Sessions.SessionCreatedDate + " has been Closed by the Consellor",
                IntendedUser = p.PatientID,
                Reason = "Notification",
            };
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            //Create alert for Counsellor
            var alert2 = new Alert()
            {
                Message = "Session, dated on " + counselling_Sessions.SessionCreatedDate + " has been Closed Successfully",
                IntendedUser = user,
                Reason = "Notification",
            };
            _context.Alerts.Add(alert2);
            await _context.SaveChangesAsync();
            //update appointment Status
            return RedirectToAction(nameof(Index));
        }
        //GET: Counselling_Sessions/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Counselling_Sessions == null)
            {
                return NotFound();
            }
            var counselling_Sessions = await _context.Counselling_Sessions.FindAsync(id);
            if (counselling_Sessions == null)
            {
                return NotFound();
            }
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentId", "Purpose", counselling_Sessions.AppointmentID);
            ViewData["CounsellorID"] = new SelectList(_context.Users, "Id", "Id", counselling_Sessions.CounsellorID);
            return View(counselling_Sessions);
        }
        // POST: Counselling_Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SessionID,SessionCreatedDate,SessionDate,SessionTime,CounsellorID,AppointmentID,Status")] Counselling_Sessions counselling_Sessions)
        {

            if (id != counselling_Sessions.SessionID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(counselling_Sessions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //private bool Counselling_SessionsExists(int id)
                    //{
                    //    return _context.Counselling_Sessions.Any(e => e.SessionID == id);
                    //}

                    if (!Counselling_SessionsExists(counselling_Sessions.SessionID))
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
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentId", "Reason", counselling_Sessions.AppointmentID);
            ViewData["CounsellorID"] = new SelectList(_context.Users, "Id", "Id", counselling_Sessions.CounsellorID);
            return View(counselling_Sessions);
        }
        // GET: Counselling_Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Counselling_Sessions == null)
            {
                return NotFound();
            }
            var counselling_Sessions = await _context.Counselling_Sessions
                .Include(c => c.Appointment)
                .Include(c => c.Counsellor)
                .FirstOrDefaultAsync(m => m.SessionID == id);
            if (counselling_Sessions != null)
            {
                _context.Counselling_Sessions.Remove(counselling_Sessions);
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Session  has been Deleted Successfully";
            //Create alert
            //find the patient
            var p = _context.Appointments.Where(a => a.AppointmentID == counselling_Sessions.AppointmentID).FirstOrDefault();
            var alert = new Alert()
            {
                Message = "Session has be delete, dated on " + counselling_Sessions.SessionCreatedDate + " Contact the Counsellor for more details",
                IntendedUser = p.PatientID,
                Reason = "Notification",

            };
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // POST: Counselling_Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Counselling_Sessions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Counselling_Sessions'  is null.");
            }
            var counselling_Sessions = await _context.Counselling_Sessions.FindAsync(id);
            if (counselling_Sessions != null)
            {
                _context.Counselling_Sessions.Remove(counselling_Sessions);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Counselling_SessionsExists(int id)
        {
            return (_context.Counselling_Sessions?.Any(e => e.SessionID == id)).GetValueOrDefault();
        }
    }
}
