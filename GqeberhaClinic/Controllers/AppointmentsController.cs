using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GqeberhaClinic.Areas.Identity.Data;
using GqeberhaClinic.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using GqeberhaClinic.Services;
using System.Text.Encodings.Web;
using GqeberhaClinic.Data;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _email;
        public AppointmentsController(ApplicationDbContext context, IEmailSender email)
        {
            _context = context;
            _email = email;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.Patients = (from U in _context.Users
                                join UR in _context.UserRoles on U.Id equals UR.UserId
                                join R in _context.Roles on UR.RoleId equals R.Id
                                where R.Name == "Patient"
                                select U).ToList();
            ViewBag.Clinic = (from U in _context.Users
                                join UR in _context.UserRoles on U.Id equals UR.UserId
                                join R in _context.Roles on UR.RoleId equals R.Id
                                where R.Name != "Patient" || R.Name == "Admin"
                              select U).ToList();
            ViewBag.Appointmnet = await _context.Appointments.Include(a => a.Patient).Where( e => e.PatientID == user).OrderByDescending(a => a.CreatedDate).ToListAsync();
            return View();
        }
        public async Task<IActionResult> All_Appointments()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.Patients = (from U in _context.Users
                                join UR in _context.UserRoles on U.Id equals UR.UserId
                                join R in _context.Roles on UR.RoleId equals R.Id
                                select U).ToList();
            ViewBag.Clinic = (from U in _context.Users
                              join UR in _context.UserRoles on U.Id equals UR.UserId
                              join R in _context.Roles on UR.RoleId equals R.Id
                              where R.Name != "Patient" || R.Name == "Admin"
                              select U).ToList();

            ViewBag.appoint = _context.Appointments.Include(a => a.Patient).OrderByDescending(a => a.CreatedDate).ToList();
            return View();
        }
        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointments == null)
            {
                return NotFound();
            }

            return View(appointments);
        }
        public IActionResult Create_Partial()
        {

            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.App = _context.Appointments.Where(a => a.PatientID == user).ToList();
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create_Partial(Appointments appointments)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (appointments.PatientID == null)
            {
        
                appointments.PatientID = user;
                if (ModelState.IsValid)
                {
                    _context.Add(appointments);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "New Appointment Has been Created Successfully";
                    TempData["UpdateType"] = "success";
                    try
                    {
                        string supportEmail = "enompilo.healthcare@gmail.com";
                        var email = User.FindFirstValue(ClaimTypes.Email);
                        await _email.SendEmailAsync(email, "Appointment Creation",
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
                          $"<p></p>" +
                          $"<p>This is Notificatio Email About Appointments</p>" +
                          $"<strong><p>Appointment Date: {appointments.Date_Time}</p></strong>" +
                          $"<p>Please Note the appopintment has been successfully created Succcessfully</p>." +

                          $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                          $"<div class='footer'>" +
                          $" <p>Thank you,</p>" +
                          $" <p>Enompilo health care Team</p>" +
                          $"</div>" +
                          $" </body>" +
                          $"</html>");

                    }
                    catch (Exception ex)
                    {
                        TempData["Success"] = "New Appointment Has been Created Successfully but email Notification failed";
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(appointments);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "New Appointment Has been Created Successfully";
                    TempData["UpdateType"] = "success";
                    try
                    {
                        string supportEmail = "enompilo.healthcare@gmail.com";
                        //find the patient
                        var p = _context.Users.Where(a => a.Id == appointments.PatientID).FirstOrDefault();
                        
                        await _email.SendEmailAsync(p.Email, "New Appointment",
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
                          $"<p></p>" +
                          $"<p>This is Notificatio Email About Appointments</p>" +
                          $"<strong><p>Appointment Date: {appointments.Date_Time}</p></strong>" +
                          $"<p>Please Note the appopintment has been successfully Created by our Nurse</p>." +

                          $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                          $"<div class='footer'>" +
                          $" <p>Thank you,</p>" +
                          $" <p>Enompilo health care Team</p>" +
                          $"</div>" +
                          $" </body>" +
                          $"</html>");
                        var alerts = new Alert()
                        {
                            Message = "Appointment for " + appointments.CreatedDate + " has been Successfully Created by our Nurse",
                            IntendedUser = appointments.PatientID,
                            Role = "Notification",
                        };

                    }
                    catch (Exception ex)
                    {
                        TempData["Success"] = "New Appointment Has been Created Successfully but email Notification failed";
                    }
                    return RedirectToAction(nameof(All_Appointments));
                }
            }


            ViewBag.App = _context.Appointments.Where(a => a.PatientID == user).ToList();
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", appointments.PatientID);
            return View(appointments);
        }
        // GET: Appointments/Create
        public IActionResult Create()
        {

            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.App = _context.Appointments.Where(a => a.PatientID == user).ToList();
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointments appointments)
        {

            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointments.PatientID = user;
            if (ModelState.IsValid)
            {
                _context.Add(appointments);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New Appointment Has been Created Successfully";
                try
                {
                    string supportEmail = "enompilo.healthcare@gmail.com";
                    var email = User.FindFirstValue(ClaimTypes.Email);
                    await _email.SendEmailAsync(email, "Appointment Creation",
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
                      $"<p></p>" +
                      $"<p>This is Notificatio Email About Appointments</p>" +
                      $"<strong><p>Appointment Date: {appointments.Date_Time}</p></strong>" +
                      $"<p>Please Note the appopintment has been successfully created Succcessfully</p>." +
                     
                      $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                      $"<div class='footer'>" +
                      $" <p>Thank you,</p>" +
                      $" <p>Enompilo health care Team</p>" +
                      $"</div>" +
                      $" </body>" +
                      $"</html>");

                }
                catch(Exception ex)
                {
                    TempData["Success"] = "New Appointment Has been Created Successfully but email Notification failed";
                }
                return RedirectToAction("Create");
            }
            ViewBag.App = _context.Appointments.Where(a => a.PatientID == user).ToList();
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", appointments.PatientID);
            return View(appointments);
        }
      public async Task<IActionResult> Accept(int? id)
        {
           
            var appointments = await _context.Appointments.FindAsync(id);
            if (appointments != null)
            {
                appointments.Status = "Accepted";
                _context.Appointments.Update(appointments);
                
                await _context.SaveChangesAsync();
                var alert = new Alert()
                {
                    Message = "Appointment made on "+appointments.CreatedDate +" for "+ appointments.Reason +  " has been Accepted",
                    IntendedUser = appointments.PatientID,
                    Role = "Notification"
                };
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Appointment has been Accepted";
                var patient = _context.Users.Where(a => a.Id == appointments.PatientID).FirstOrDefault();
                try
                {
                    string supportEmail = "enompilo.healthcare@gmail.com";
                    await _email.SendEmailAsync(patient.Email, "Appointment Accepted",
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
                      $"<p></p>" +
                      $"<p>This is Notificatio Email About Appointments</p>" +
                      $"<strong><p>Appointment Date and Time: {appointments.Date_Time}</p></strong>" +
                      $"<strong><p>Appointment Reason: {appointments.Reason}</p></strong>" +
                      $"<p>Please Note the appopintment has been successfully Accepted by our Nurses</p>." +

                      $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                      $"<div class='footer'>" +
                      $" <p>Thank you,</p>" +
                      $" <p>Enompilo health care Team</p>" +
                      $"</div>" +
                      $" </body>" +
                      $"</html>"
                        );
                }
                catch
                {

                }
                return RedirectToAction("All_Appointments");
            }
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", appointments.PatientID);
            return View(appointments);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments.FindAsync(id);
            if (appointments == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", appointments.PatientID);
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edited(Appointments appointments)
        {
            if (appointments.AppointmentID != null)
            {
                try
                {
                    var appoint = _context.Appointments.Where(a => a.AppointmentID == appointments.AppointmentID).FirstOrDefault();
                    appoint.Date_Time = appointments.Date_Time;
                    _context.Update(appoint);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Appointment has been Edited";
                    TempData["UpdateType"] = "success";
                    var patient = _context.Users.Where(a => a.Id == appoint.PatientID).FirstOrDefault();
                    try
                    {
                        string supportEmail = "enompilo.healthcare@gmail.com";
                        await _email.SendEmailAsync(patient.Email, "Appointment Accepted",
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
                          $"<p></p>" +
                          $"<p>This is Notificatio Email About Appointments</p>" +
                          $"<strong><p>Appointment Date and Time: {appointments.Date_Time}</p></strong>" +
                          $"<strong><p>Appointment Reason: {appointments.Reason}</p></strong>" +
                          $"<p>Please Note the appopintment has been Postponed by our Nurses</p>." +

                          $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                          $"<div class='footer'>" +
                          $" <p>Thank you,</p>" +
                          $" <p>Enompilo health care Team</p>" +
                          $"</div>" +
                          $" </body>" +
                          $"</html>"
                            );
                        return RedirectToAction("All_Appointments");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AppointmentsExists(appointments.AppointmentID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
                catch
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["Success"] = "Something went wrong";
            TempData["UpdateType"] = "danger";
            return RedirectToAction("All_Appointments");
        }
        public async Task<IActionResult> Edit(Appointments appointments)
        {
           if(appointments.AppointmentID != null)
            {
                try
                {
                    var appoint = _context.Appointments.Where(a => a.AppointmentID == appointments.AppointmentID).FirstOrDefault();
                    appoint.Date_Time = appointments.Date_Time;
                    appoint.Status = "Postponed";
                    _context.Update(appoint);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Appointment has been PostPoned";
                    TempData["UpdateType"] = "success"; 
                    var patient = _context.Users.Where(a => a.Id == appoint.PatientID).FirstOrDefault();
                    try
                    {
                        string supportEmail = "enompilo.healthcare@gmail.com";
                        await _email.SendEmailAsync(patient.Email, "Appointment Accepted",
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
                          $"<p></p>" +
                          $"<p>This is Notificatio Email About Appointments</p>" +
                          $"<strong><p>Appointment Date and Time: {appointments.Date_Time}</p></strong>" +
                          $"<strong><p>Appointment Reason: {appointments.Reason}</p></strong>" +
                          $"<p>Please Note the appopintment has been Postponed by our Nurses</p>." +

                          $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at {supportEmail}.</p>" +
                          $"<div class='footer'>" +
                          $" <p>Thank you,</p>" +
                          $" <p>Enompilo health care Team</p>" +
                          $"</div>" +
                          $" </body>" +
                          $"</html>"
                            );
                        return RedirectToAction("All_Appointments");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AppointmentsExists(appointments.AppointmentID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                   
                }
                catch
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["Success"] = "Something went wrong";
            TempData["UpdateType"] = "danger";
            return RedirectToAction("All_Appointments");
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointments == null)
            {
                return NotFound();
            }

            return View(appointments);
        }  
        public async Task<IActionResult> Delete_Appointment(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointments == null)
            {
                return NotFound();
            }
            _context.Appointments.Remove(appointments);
            await _context.SaveChangesAsync();
            //Alert the patient
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var alerts = new Alert()
            {
                Message = "Appointment for "+ appointments.CreatedDate+" has been Successfully Deleted",
                IntendedUser = user,
                Role = "Notification",
            };
            _context.Alerts.Add(alerts);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Appointment Successfully Deleted";
            TempData["UpdateType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointments'  is null.");
            }
            var appointments = await _context.Appointments.FindAsync(id);
            if (appointments != null)
            {
                _context.Appointments.Remove(appointments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentsExists(int id)
        {
          return (_context.Appointments?.Any(e => e.AppointmentID == id)).GetValueOrDefault();
        }
        /// <summary>
        /// Appointments for Counselling
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Counselling()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Alerts = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            if (Alerts.Count > 0)
            {
                ViewBag.Alerts = Alerts;
                TempData["Alerts"] = "Not null";
            }
            ViewBag.Appointment = _context.Appointments.Where(a => (a.Reason == "Counselling" && a.PatientID == user)).ToList();
            return View();
        }
        public async Task<IActionResult> Counselling_Appointments()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Alerts = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            if (Alerts.Count > 0)
            {
                ViewBag.Alerts = Alerts;
                TempData["Alerts"] = "Not null";
            }
            ViewBag.Appointment = _context.Appointments.Where(a => a.Reason == "Counselling").Include(a => a.Patient).ToList();
            return View();
        }
        // GET: Appointments/Create
        public IActionResult Counselling_Appointment()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Counselling_Appointment(Appointments appointments)
        {

            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointments.PatientID = user;
            appointments.Reason = "Counselling";
            if (appointments.Date_Time != null && appointments.Reason != null)
            {
                _context.Add(appointments);
                await _context.SaveChangesAsync();
                //after successfully creating the appoint notify the user
                TempData["Success"] = "New Appointment has been Successfully Added";
                //Create alert
                try
                {
                    var alert = new Alert()
                    {
                        Message = "New appointment has been Created Date: "+ appointments.CreatedDate,
                        IntendedUser = user,
                        Role = "Notification",

                    };
                    _context.Alerts.Add(alert);
                    await _context.SaveChangesAsync();

                    //Send email
                    var email = User.FindFirstValue(ClaimTypes.Email);
                    await _email.SendEmailAsync(email, "New Appointment",

                        $"<html> <head> <style> body {{ font-family: Arial, sans-serif; }} " +
                        $"h1 {{ color: #336699; }}" +
                        $".cta-button {{ background-color: #008CBA; color: white;" +
                        $" padding: 10px 20px;" +
                        $" text-decoration: none; border-radius: 5px; }}" +
                        $".cta-button:hover {{ background-color: #265580; }}" +
                        $".footer {{ margin-top: 20px; font-size: 12px; color: #888; }}" +
                        $"  </style>" +
                        $"</head>" +
                        $"<body>" +
                        $"" +
                        $"<p>Dear Patient,</p>" +
                        $"<p>this is a confimation email that your Appointment has been Successfully Created.</p>" +
                        $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team</p>" +
                        $"<div class='footer'>" +
                        $" <p>Thank you,</p>" +
                        $" <p>Healthcare Team</p>" +
                        $"</div>" +
                        $" </body>" +
                        $"</html>");
                    return RedirectToAction(nameof(Counselling));
                }
                catch (Exception ex) { }


            }
            else
            {
                TempData["Success"] = "Something went Wrong Try Again";
                return RedirectToAction(nameof(Counselling));
            }
            return RedirectToAction(nameof(Counselling));

        }
        // GET: Appointments/Delete/5
        public async Task<IActionResult> Accept_Conselling_Appointment(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }
            try
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var appointments = await _context.Appointments
                    .FirstOrDefaultAsync(m => m.AppointmentID == id);
                appointments.Status = "Accepted";
                _context.Appointments.Update(appointments);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Appointment has been Accepted";
                //Create alert
                var alert = new Alert()
                {
                    Message = "Please note that appointment, dated on " + appointments.CreatedDate + " has been accepted by one of our Consellor",
                    IntendedUser = appointments.PatientID,
                    Role = "Notification",

                };
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                //Send
                //Find the patient Email
                var Pemail = _context.Users.Where(a => a.Id == appointments.PatientID).FirstOrDefault();
                var email = User.FindFirstValue(ClaimTypes.Email);
                await _email.SendEmailAsync(Pemail.Email, "Appointment Accepted notification",

                    $"<html> <head> <style> body {{ font-family: Arial, sans-serif; }} " +
                    $"h1 {{ color: #336699; }}" +
                    $".cta-button {{ background-color: #008CBA; color: white;" +
                    $" padding: 10px 20px;" +
                    $" text-decoration: none; border-radius: 5px; }}" +
                    $".cta-button:hover {{ background-color: #265580; }}" +
                    $".footer {{ margin-top: 20px; font-size: 12px; color: #888; }}" +
                    $"  </style>" +
                    $"</head>" +
                    $"<body>" +
                    $"" +
                    $"<p>Dear  Patient,</p>" +
                    $"<p>this is a confimation email that your Appointment has been Successfully Accepted by one of our Counsellor.</p>" +
                    $"<p>Be in the look for a Session with the Counsellor</p>" +
                    $"<p>If you have any questions, please don't hesitate to contact our team</p>" +
                    $"<div class='footer'>" +
                    $" <p>Thank you,</p>" +
                    $" <p>MedicalLifeHealthCare Team</p>" +
                    $"</div>" +
                    $" </body>" +
                    $"</html>");
                return RedirectToAction(nameof(Counselling_Appointments));

            }
            catch (Exception ex)
            {
                TempData["Success"] = "Something went Wrong Try Again";
                return RedirectToAction(nameof(Counselling_Appointments));
            }

        }
        public async Task<IActionResult> Delete_Conselling_Appointment(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }
            try
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var appointments = await _context.Appointments
                    .FirstOrDefaultAsync(m => m.AppointmentID == id);
                _context.Appointments.Remove(appointments);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Appointment has been deleted";
                //Create alert
                var alert = new Alert()
                {
                    Message = "Appointment has been Deleted",
                    IntendedUser = user,
                    Role = "Notification",

                };
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Counselling));

            }
            catch (Exception ex)
            {
                TempData["Success"] = "Something went Wrong Try Again";
                return RedirectToAction(nameof(Counselling));
            }

        }
    }
}
