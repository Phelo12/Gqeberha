using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GqeberhaClinic.Areas.Identity.Data;
using GqeberhaClinic.Models;
using GqeberhaClinic.Migrations;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;
using GqeberhaClinic.Data;

namespace GqeberhaClinic.Controllers
{
    public class MedicalRefillsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _email;
        public MedicalRefillsController(ApplicationDbContext context, IEmailSender email)
        {
            _context = context;
            _email = email;
        }

        // GET: MedicalRefills
        public async Task<IActionResult> All_Requests(int ID)
        {
            if(ID == null || ID == 0)
            {
                ViewBag.List = _context.MedicalRefill.Include(m => m.Doctor).Include(m => m.Patient).Include(m => m.Prescription).Include(a => a.Prescription.Doctor).OrderByDescending(a => a.RequestDate).ToList();
                return View();
            }
            else
            {
                ViewBag.List = _context.MedicalRefill.Include(m => m.Doctor).Include(m => m.Patient).Include(m => m.Prescription).Include(a => a.Prescription.Doctor).Where(a => a.PrescriptionId == ID).OrderByDescending(a => a.RequestDate).ToList();
                return View();
            }
         
        } public async Task<IActionResult> Index(int ID)
        {
            if(ID == null || ID == 0)
            {
                ViewBag.List = _context.MedicalRefill.Include(m => m.Doctor).Include(m => m.Patient).Include(m => m.Prescription).Include(a => a.Prescription.Doctor).ToList();
                return View();
            }
            else
            {
                ViewBag.List = _context.MedicalRefill.Include(m => m.Doctor).Include(m => m.Patient).Include(m => m.Prescription).Include(a => a.Prescription.Doctor).Where(a => a.PrescriptionId == ID).ToList();
                return View();
            }
         
        }

        // GET: MedicalRefills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MedicalRefill == null)
            {
                return NotFound();
            }

            var medicalRefill = await _context.MedicalRefill
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Include(m => m.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRefill == null)
            {
                return NotFound();
            }

            return View(medicalRefill);
        }

        // GET: MedicalRefills/Create
        public IActionResult Create()
        {
            ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PrescriptionId"] = new SelectList(_context.Prescription, "Id", "Dosage");
            return View();
        }

        // POST: MedicalRefills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRefill medicalRefill)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            medicalRefill.PatientID = user;
            if (ModelState.IsValid)
            {
                _context.Add(medicalRefill);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New Medical refill Has been Created Successfully";
                TempData["UpdateType"] = "success";
                try
                {
                 
                    var prec = _context.Prescription.Where(a => a.Id == medicalRefill.PrescriptionId).FirstOrDefault();
                    var doc = _context.Users.Where(a => a.Id == prec.DoctorsID).FirstOrDefault();

                    var alerts = new Alert()
                    {
                        Message = "Medical Refill has been Request  by on of our patients Patients.",
                        IntendedUser = prec.DoctorsID,
                        Role = "Notification",
                    };

                    return RedirectToAction(nameof(Index));
                }
                catch
                {

                }
            }
            else
            {
                ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id", medicalRefill.DoctorsID);
                ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", medicalRefill.PatientID);
                ViewData["PrescriptionId"] = new SelectList(_context.Prescription, "Id", "Dosage", medicalRefill.PrescriptionId);
                return View(medicalRefill);
            }
            return View(medicalRefill);

        }

        // GET: MedicalRefills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MedicalRefill == null)
            {
                return NotFound();
            }

            var medicalRefill = await _context.MedicalRefill.FindAsync(id);
            if (medicalRefill == null)
            {
                return NotFound();
            }
            ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id", medicalRefill.DoctorsID);
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", medicalRefill.PatientID);
            ViewData["PrescriptionId"] = new SelectList(_context.Prescription, "Id", "Dosage", medicalRefill.PrescriptionId);
            return View(medicalRefill);
        }

        // POST: MedicalRefills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicalRefill medicalRefill)
        {
            if (id != medicalRefill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var info = _context.MedicalRefill.Where(a => a.Id == medicalRefill.Id).FirstOrDefault();
                    info.QuantityRequested = medicalRefill.QuantityRequested;
                    info.Notes = medicalRefill.Notes;
                    _context.Update(info);
                    TempData["Success"] = "Request  Has been updated Successfully";
                    TempData["UpdateType"] = "success";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRefillExists(medicalRefill.Id))
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
            ViewData["DoctorsID"] = new SelectList(_context.Users, "Id", "Id", medicalRefill.DoctorsID);
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", medicalRefill.PatientID);
            ViewData["PrescriptionId"] = new SelectList(_context.Prescription, "Id", "Dosage", medicalRefill.PrescriptionId);
            return View(medicalRefill);
        }

        // GET: MedicalRefills/Delete/5
        public async Task<IActionResult> Decline(int? id)
        {
            if (id == null || _context.MedicalRefill == null)
            {
                return NotFound();
            }

            var medicalRefill = await _context.MedicalRefill
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Include(m => m.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRefill == null)
            {
                return NotFound();
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            medicalRefill.Status = RefillStatus.Decliend;
            medicalRefill.ApprovalDate = DateTime.Now;
            medicalRefill.DoctorsID = user;
            _context.MedicalRefill.Update(medicalRefill);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Request  Has been Declined Successfully";
            TempData["UpdateType"] = "success";
            return RedirectToAction(nameof(All_Requests));
        }            
        public async Task<IActionResult> Packaging(int? id)
        {
            if (id == null || _context.MedicalRefill == null)
            {
                return NotFound();
            }

            var medicalRefill = await _context.MedicalRefill
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Include(m => m.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRefill == null)
            {
                return NotFound();
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            medicalRefill.Status = RefillStatus.Packaging;
            medicalRefill.ApprovalDate = DateTime.Now;
            medicalRefill.DoctorsID = user;
            _context.MedicalRefill.Update(medicalRefill);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Request  is in Packaging Mode";
            TempData["UpdateType"] = "success";
            return RedirectToAction(nameof(All_Requests));
        }     
        public async Task<IActionResult> Collection(int? id)
        {
            if (id == null || _context.MedicalRefill == null)
            {
                return NotFound();
            }

            var medicalRefill = await _context.MedicalRefill
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Include(m => m.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRefill == null)
            {
                return NotFound();
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            medicalRefill.Status = RefillStatus.Ready_For_Collection;
            medicalRefill.ApprovalDate = DateTime.Now;
            medicalRefill.DoctorsID = user;
            _context.MedicalRefill.Update(medicalRefill);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Request  is ready for Collection";
            TempData["UpdateType"] = "success";
            return RedirectToAction(nameof(All_Requests));
        }      public async Task<IActionResult> Accept(int? id)
        {
            if (id == null || _context.MedicalRefill == null)
            {
                return NotFound();
            }

            var medicalRefill = await _context.MedicalRefill
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Include(m => m.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRefill == null)
            {
                return NotFound();
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            medicalRefill.Status = RefillStatus.Approved;
            medicalRefill.ApprovalDate = DateTime.Now;
            medicalRefill.DoctorsID = user;
            _context.MedicalRefill.Update(medicalRefill);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Request  Has been Approved Successfully";
            TempData["UpdateType"] = "success";
            return RedirectToAction(nameof(All_Requests));
        }    
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MedicalRefill == null)
            {
                return NotFound();
            }

            var medicalRefill = await _context.MedicalRefill
                .Include(m => m.Doctor)
                .Include(m => m.Patient)
                .Include(m => m.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRefill == null)
            {
                return NotFound();
            }
            _context.MedicalRefill.Remove(medicalRefill);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Request  Has been Deleted Successfully";
            TempData["UpdateType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // POST: MedicalRefills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MedicalRefill == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MedicalRefill'  is null.");
            }
            var medicalRefill = await _context.MedicalRefill.FindAsync(id);
            if (medicalRefill != null)
            {
                _context.MedicalRefill.Remove(medicalRefill);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalRefillExists(int id)
        {
          return (_context.MedicalRefill?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
