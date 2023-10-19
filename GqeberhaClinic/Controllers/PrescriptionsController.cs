using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using GqeberhaClinic.Data;
using GqeberhaClinic.Models;

namespace MedicalLifeHealthcare.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _email;
        public PrescriptionsController(ApplicationDbContext context, IEmailSender email)
        {
            _context = context;
            _email = email;
        }

        // GET: Prescriptions
        public async Task<IActionResult> My_Prescription_Report()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderBy(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.Date = DateTime.Now.ToString("dd/MMMM/yyyy");
            ViewBag.Time = DateTime.Now.ToString("HH:MM");
            ViewBag.Patients = (from U in _context.Users
                                join UR in _context.UserRoles on U.Id equals UR.UserId
                                join R in _context.Roles on UR.RoleId equals R.Id
                                where R.Name == "Patient"
                                select U).ToList();
            ViewBag.Pre = _context.Prescription.Include(p => p.Doctor).Include(p => p.Patient).Where(a => a.PatientId == user).ToList();
            return View();
        }    public async Task<IActionResult> My_Prescription()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderBy(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.Patients = (from U in _context.Users
                                join UR in _context.UserRoles on U.Id equals UR.UserId
                                join R in _context.Roles on UR.RoleId equals R.Id
                                where R.Name == "Patient"
                                select U).ToList();
            ViewBag.Pre = _context.Prescription.Include(p => p.Doctor).Include(p => p.Patient).Where(a => a.PatientId == user).ToList();
            return View();
        }   
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderBy(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.Patients = (from U in _context.Users
                                join UR in _context.UserRoles on U.Id equals UR.UserId
                                join R in _context.Roles on UR.RoleId equals R.Id
                                where R.Name == "Patient"
                                select U).ToList();
            ViewBag.Pre = _context.Prescription.Include(p => p.Doctor).Include(p => p.Patient).ToList();
            return View();
        }

        // GET: Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prescription == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescription
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // GET: Prescriptions/Create
        public IActionResult Create()
        {
            ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Prescription prescription)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderBy(a => a.Date).ToList();
            ViewBag.Alert = alert;
            prescription.DoctorsID = user;
            if (ModelState.IsValid)
            {
                _context.Add(prescription);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New Prescriprion Has been Created Successfully";
                TempData["UpdateType"] = "success";
                try
                {
                    string supportEmail = "enompilo.healthcare@gmail.com";
                    //find the patient
                    var p = _context.Users.Where(a => a.Id == prescription.PatientId).FirstOrDefault();

                    await _email.SendEmailAsync(p.Email, "New Prescription",
                      $"<html> <head> <style> body {{ font-family: Arial, sans-serif; }} " +
                      $"h1 {{ color: #00347C; }}" +
                      $".cta-button {{ background-color: #00C2CC; color: white;" +
                      $" padding: 10px 20px;" +
                      $" text-decoration: none; border-radius: 5px; }}" +
                      $".cta-button:hover {{ background-color: #00C2CC; }}" +
                      $".footer {{ margin-top: 20px; font-size: 12px; color: #888; }}" +
                      $"  </style>" +
                      $"</head>" +
                      $"<body>" +
                      $"" +
                      $"<h1>Enompilo Health Care!</h1>" +
                      $"<p>Dear {p.FirstName} {p.LastName}</p>" +
                      $"<p>This is Notificatio Email About New Prescription</p>" +
                      $"<strong><p>Medication Name: {prescription.MedicationName}</p></strong>" +
                      $"<strong><p>Dosage: {prescription.Dosage}</p></strong>" +
                     
                      $"<p>Please Note the Prescreption  has been successfully Created by our Doctor</p>." +

                      $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                      $"<div class='footer'>" +
                      $" <p>Thank you,</p>" +
                      $" <p>Enompilo health care Team</p>" +
                      $"</div>" +
                      $" </body>" +
                      $"</html>");
                    var alerts = new Alert()
                    {
                        Message = "Medical Prescription has been Created for you by our doctor.",
                        IntendedUser = prescription.PatientId,
                        Role = "Notification",
                    };

                }
                catch (Exception ex)
                {
                    TempData["Success"] = "New Prescription Has been Created Successfully but email Notification failed";
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id", prescription.DoctorsID);
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", prescription.PatientId);
            return View(prescription);
        }

        // GET: Prescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prescription == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescription.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id", prescription.DoctorsID);
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", prescription.PatientId);
            return View(prescription);
        }

        // POST: Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatePrescribed,MedicationName,Dosage,Duration,Notes,DoctorsID,PatientId")] Prescription prescription)
        {
            if (id != prescription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "New Prescriprion Has been Edited  Successfully";
                    TempData["UpdateType"] = "success";
                    try
                    {
                        string supportEmail = "enompilo.healthcare@gmail.com";
                        //find the patient
                        var p = _context.Users.Where(a => a.Id == prescription.PatientId).FirstOrDefault();

                        await _email.SendEmailAsync(p.Email, "New Prescription",
                          $"<html> <head> <style> body {{ font-family: Arial, sans-serif; }} " +
                          $"h1 {{ color: #00347C; }}" +
                          $".cta-button {{ background-color: #00C2CC; color: white;" +
                          $" padding: 10px 20px;" +
                          $" text-decoration: none; border-radius: 5px; }}" +
                          $".cta-button:hover {{ background-color: #00C2CC; }}" +
                          $".footer {{ margin-top: 20px; font-size: 12px; color: #888; }}" +
                          $"  </style>" +
                          $"</head>" +
                          $"<body>" +
                          $"" +
                          $"<h1>Enompilo Health Care!</h1>" +
                          $"<p>Dear {p.FirstName} {p.LastName}</p>" +
                          $"<p>This is Notificatio Email About  Prescription</p>" +
                          $"<strong><p>Medication Name: {prescription.MedicationName}</p></strong>" +
                          $"<strong><p>Dosage: {prescription.Dosage}</p></strong>" +

                          $"<p>Please Note the Prescreption  has been successfully Edited by our Doctor</p>." +

                          $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                          $"<div class='footer'>" +
                          $" <p>Thank you,</p>" +
                          $" <p>Enompilo health care Team</p>" +
                          $"</div>" +
                          $" </body>" +
                          $"</html>");
                        var alerts = new Alert()
                        {
                            Message = "Medical Prescription has been Edited by our doctor.",
                            IntendedUser = prescription.PatientId,
                            Role = "Notification",
                        };

                    }
                    catch (Exception ex)
                    {
                        TempData["Success"] = "New Prescription Has been Edited Successfully but email Notification failed";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionExists(prescription.Id))
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
            ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id", prescription.DoctorsID);
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", prescription.PatientId);
            return View(prescription);
        }

        // GET: Prescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prescription == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescription
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }
            _context.Prescription.Remove(prescription);
            await _context.SaveChangesAsync();
            TempData["Success"] = " Prescriprion Has been Deleted  Successfully";
            TempData["UpdateType"] = "success";  
            return RedirectToAction(nameof(Index));
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prescription == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Prescription'  is null.");
            }
            var prescription = await _context.Prescription.FindAsync(id);
            if (prescription != null)
            {
                _context.Prescription.Remove(prescription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(int id)
        {
          return (_context.Prescription?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
