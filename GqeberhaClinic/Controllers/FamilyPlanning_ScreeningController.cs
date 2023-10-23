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
using GqeberhaClinic.Areas.Identity.Data;

namespace GqeberhaClinic.Controllers
{
    public class FamilyPlanning_ScreeningController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public FamilyPlanning_ScreeningController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: FamilyPlanning_Screening
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.FamilyPlanning_Screening.Include(f => f.MainUser).Where(a => a.PatientID == user);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FamilyPlanning_Screening/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FamilyPlanning_Screening == null)
            {
                return NotFound();
            }

            var familyPlanning_Screening = await _context.FamilyPlanning_Screening
                .Include(f => f.MainUser)
                .FirstOrDefaultAsync(m => m.ScreeningID == id);
            if (familyPlanning_Screening == null)
            {
                return NotFound();
            }

            return View(familyPlanning_Screening);
        }

        // GET: FamilyPlanning_Screening/Create
        public IActionResult Create()
        {
          
            return View();
        }

        // POST: FamilyPlanning_Screening/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FamilyPlanning_Screening familyPlanning_Screening)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Email = User.FindFirstValue(ClaimTypes.Email);
            familyPlanning_Screening.PatientID = user;
            int total = 0;
            if (ModelState.IsValid)
            {
                total += Convert.ToInt32(familyPlanning_Screening.Question1);
                total += Convert.ToInt32(familyPlanning_Screening.Question2);
                total += Convert.ToInt32(familyPlanning_Screening.Question3);
                total += Convert.ToInt32(familyPlanning_Screening.Question4);
                total += Convert.ToInt32(familyPlanning_Screening.Question5);
                total += Convert.ToInt32(familyPlanning_Screening.Question6);
                total += Convert.ToInt32(familyPlanning_Screening.Question7);
                total += Convert.ToInt32(familyPlanning_Screening.Question8);
                total += Convert.ToInt32(familyPlanning_Screening.Question9);
                total += Convert.ToInt32(familyPlanning_Screening.Question10);
                if(total < 30)
                {
                    
                    TempData["Result"] = " Consider progestin-only pills or other non-estrogen methods.";
                    familyPlanning_Screening.Message = " Consider progestin-only pills or other non-estrogen methods.";
                }
                else if(total > 30 && total < 61)
                {
                    TempData["Result"] = "Consider non-hormonal methods and barrier methods like condoms.";
                    familyPlanning_Screening.Message = "Consider non-hormonal methods and barrier methods like condoms.";
                }
                else if (total > 61)
                {
                    TempData["Result"] = "Birth control pills might be suitable.";
                    familyPlanning_Screening.Message = "Birth control pills might be suitable.";
                }
                familyPlanning_Screening.Total = total;
                _context.Add(familyPlanning_Screening);
                try{
                    await _emailSender.SendEmailAsync(User.FindFirstValue(ClaimTypes.Email), "Screening Results",
                        "Screening has been done");
                    var alert = new Alert()
                    {
                        Message = "Screening has been done sucessfully",
                        IntendedUser = user,
                        Role = "notification",

                    };
                    _context.Alerts.Add(alert);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    //var admins = (from U in _context.Users
                    //              join UR in _context.UserRoles on U.Id equals UR.UserId
                    //              join R in _context.Roles on UR.RoleId equals R.Id
                    //              where R.Name == "Admin"
                    //              select new GqebheraUser { }).ToList();
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", familyPlanning_Screening.PatientID);
            return View(familyPlanning_Screening);
        }

        // GET: FamilyPlanning_Screening/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FamilyPlanning_Screening == null)
            {
                return NotFound();
            }

            var familyPlanning_Screening = await _context.FamilyPlanning_Screening.FindAsync(id);
            if (familyPlanning_Screening == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", familyPlanning_Screening.PatientID);
            return View(familyPlanning_Screening);
        }

        // POST: FamilyPlanning_Screening/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScreeningID,Date,PatientID,Status,Question1,Question2,Question3,Question4,Question5,Question6,Question7,Question8,Question9,Question10")] FamilyPlanning_Screening familyPlanning_Screening)
        {
            if (id != familyPlanning_Screening.ScreeningID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(familyPlanning_Screening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyPlanning_ScreeningExists(familyPlanning_Screening.ScreeningID))
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
            ViewData["PatientID"] = new SelectList(_context.Users, "Id", "Id", familyPlanning_Screening.PatientID);
            return View(familyPlanning_Screening);
        }

        // GET: FamilyPlanning_Screening/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FamilyPlanning_Screening == null)
            {
                return NotFound();
            }

            var familyPlanning_Screening = await _context.FamilyPlanning_Screening
                .Include(f => f.MainUser)
                .FirstOrDefaultAsync(m => m.ScreeningID == id);
            if (familyPlanning_Screening == null)
            {
                return NotFound();
            }
            _context.FamilyPlanning_Screening.Remove(familyPlanning_Screening);

            await _context.SaveChangesAsync();
            TempData["Success"] = "History has been Deleted";
            TempData["UpdateType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // POST: FamilyPlanning_Screening/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FamilyPlanning_Screening == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FamilyPlanning_Screening'  is null.");
            }
            var familyPlanning_Screening = await _context.FamilyPlanning_Screening.FindAsync(id);
            if (familyPlanning_Screening != null)
            {
                _context.FamilyPlanning_Screening.Remove(familyPlanning_Screening);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyPlanning_ScreeningExists(int id)
        {
          return (_context.FamilyPlanning_Screening?.Any(e => e.ScreeningID == id)).GetValueOrDefault();
        }
    }
}
