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
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GqeberhaClinic.Controllers
{
    public class Medical_FileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public Medical_FileController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Medical_File
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            var applicationDbContext = _context.Medical_File.Include(m => m.mainUser);
            ViewBag.date = DateTime.Now.ToString("dd/MMM/yyyy HH:MM");
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Patient_File(int? File)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            if(File != null)
            {
                ViewBag.File = _context.Medical_File.Where(a => a.FileID == File).Include(m => m.mainUser).ToList();
            }
            
            return View();
        } public async Task<IActionResult> My_File()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.File = _context.Medical_File.Where(a => a.PatientID == user).Include(m => m.mainUser).ToList();
            return View();
        }

        // GET: Medical_File/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medical_File == null)
            {
                return NotFound();
            }

            var medical_File = await _context.Medical_File
                .Include(m => m.mainUser)
                .FirstOrDefaultAsync(m => m.FileID == id);
            if (medical_File == null)
            {
                return NotFound();
            }

            return View(medical_File);
        }

        // GET: Medical_File/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id");
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            return View();
        }
        public IActionResult Create_File()
        {
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id");
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create_File([Bind("FileID,PatientID,Gender,BirthDate,IDNumber,AddressLine1,Province,Country,PostalCode,EmergencyPerson,EmergencyContactNo,Relationship,BloodType,Allergies,AnySurgeries,ExtraNotes")] Medical_File medical_File)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            medical_File.PatientID = user;
            if (ModelState.IsValid)
            {
                _context.Add(medical_File);
                await _context.SaveChangesAsync();
                try
                {
                    var alerts = new Alert()
                    {
                        Message = "New Medical File has been Created.",
                        Role = "Notify",
                        IntendedUser = user,
                    };
                    _context.Alerts.Add(alerts);
                    await _context.SaveChangesAsync();
                    try
                    {
                        var logUser = _context.Users.Where(a => a.Id == user).FirstOrDefault();
                        await _emailSender.SendEmailAsync(logUser.Email, "Medical File Created", "Please note that the file has been created");
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
                return RedirectToAction(nameof(My_File));
            }
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", medical_File.PatientID);
            //return RedirectToAction("Patient", "Walk_In", medical_File);
            TempData["Error"] = "Error";
            return View(medical_File);

        }

        // POST: Medical_File/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileID,PatientID,Gender,BirthDate,IDNumber,AddressLine1,Province,Country,PostalCode,EmergencyPerson,EmergencyContactNo,Relationship,BloodType,Allergies,AnySurgeries,ExtraNotes")] Medical_File medical_File)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medical_File);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", medical_File.PatientID);
            return View(medical_File);
        }

        // GET: Medical_File/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medical_File == null)
            {
                return NotFound();
            }

            var medical_File = await _context.Medical_File.FindAsync(id);
            if (medical_File == null)
            {
                return NotFound();
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", medical_File.PatientID);
            return View(medical_File);
        }

        // POST: Medical_File/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medical_File medical_File)
        {
            if (id != medical_File.FileID)
            {
                return NotFound();
            }
            var file = _context.Medical_File.Where(a => a.FileID == id).FirstOrDefault();
            medical_File.IDNumber = file?.IDNumber;
            medical_File.Gender = file?.Gender;
            try
            {

                file.AddressLine1 = medical_File.AddressLine1;
                file.Province = medical_File.Province;
                file.Country = medical_File.Country;
                file.PostalCode = medical_File.PostalCode;
                file.EmergencyPerson = medical_File.EmergencyPerson;
                file.EmergencyContactNo = medical_File.EmergencyContactNo;
                file.Relationship = medical_File.Relationship;
                file.BloodType = medical_File.BloodType;
                file.Allergies = medical_File.Allergies;
                file.ExtraNotes = medical_File.ExtraNotes;
                _context.Update(file);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Medical File has been Update";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Medical_FileExists(medical_File.FileID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(My_File));

        }

        // GET: Medical_File/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medical_File == null)
            {
                return NotFound();
            }

            var medical_File = await _context.Medical_File
                .Include(m => m.mainUser)
                .FirstOrDefaultAsync(m => m.FileID == id);
            if (medical_File == null)
            {
                return NotFound();
            }

            return View(medical_File);
        }

        // POST: Medical_File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medical_File == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Medical_File'  is null.");
            }
            var medical_File = await _context.Medical_File.FindAsync(id);
            if (medical_File != null)
            {
                _context.Medical_File.Remove(medical_File);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Medical_FileExists(int id)
        {
            return (_context.Medical_File?.Any(e => e.FileID == id)).GetValueOrDefault();
        }
    }
}
