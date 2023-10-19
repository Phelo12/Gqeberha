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
    public class RecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Records
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
                ViewBag.Alert = alert;
                var applicationDbContext = _context.Records.Include(r => r.File).Include(r => r.Nurse).Include(a => a.File.mainUser);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
                ViewBag.Alert = alert;
                var applicationDbContext = _context.Records.Include(r => r.File).Include(r => r.Nurse).Include(a => a.File.mainUser).Where(a => a.FileID == id);
                return View(await applicationDbContext.ToListAsync());
            }
        
        }
        public async Task<IActionResult> My_Records()
        {
           
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
                ViewBag.Alert = alert;
                var applicationDbContext = _context.Records.Include(r => r.File).Include(r => r.Nurse).Include(a => a.File.mainUser).Where(a => a.File.PatientID == user);
                return View(await applicationDbContext.ToListAsync());
            
        
        }
        public async Task<IActionResult> Report()
        {
           
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
                ViewBag.Alert = alert;
                var applicationDbContext = _context.Records.Include(r => r.File).Include(r => r.Nurse).Include(a => a.File.mainUser);
                return View(await applicationDbContext.ToListAsync());
          

        }

        // GET: Records/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var records = await _context.Records
                .Include(r => r.File)
                .Include(r => r.Nurse)
                .FirstOrDefaultAsync(m => m.RecordsID == id);
            if (records == null)
            {
                return NotFound();
            }

            return View(records);
        }

        // GET: Records/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? FileID)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            if(FileID != null)
            {
                ViewBag.Id = FileID;
            }
            ViewBag.Id = "Null";
            ViewData["FileID"] = new SelectList(_context.Medical_File, "FileID", "AddressLine1");
            ViewData["NurseID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecordsID,FileID,BloodPressure,HeartRate,Temperature,Height,Weight,Notes,RecordDate,NurseID")] Records records)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            records.NurseID = user;
            if (ModelState.IsValid)
            {
                _context.Add(records);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FileID"] = new SelectList(_context.Medical_File, "FileID", "AddressLine1", records.FileID);
            ViewData["NurseID"] = new SelectList(_context.Users, "Id", "Id", records.NurseID);
            return View(records);
        }

        // GET: Records/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var records = await _context.Records.FindAsync(id);
            if (records == null)
            {
                return NotFound();
            }
            ViewData["FileID"] = new SelectList(_context.Medical_File, "FileID", "AddressLine1", records.FileID);
            ViewData["NurseID"] = new SelectList(_context.Users, "Id", "Id", records.NurseID);
            return View(records);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordsID,FileID,BloodPressure,HeartRate,Temperature,Height,Weight,Notes,RecordDate,NurseID")] Records records)
        {
            if (id != records.RecordsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(records);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordsExists(records.RecordsID))
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
            ViewData["FileID"] = new SelectList(_context.Medical_File, "FileID", "AddressLine1", records.FileID);
            ViewData["NurseID"] = new SelectList(_context.Users, "Id", "Id", records.NurseID);
            return View(records);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var records = await _context.Records
                .Include(r => r.File)
                .Include(r => r.Nurse)
                .FirstOrDefaultAsync(m => m.RecordsID == id);
            if (records == null)
            {
                return NotFound();
            }

            return View(records);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Records == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Records'  is null.");
            }
            var records = await _context.Records.FindAsync(id);
            if (records != null)
            {
                _context.Records.Remove(records);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordsExists(int id)
        {
          return (_context.Records?.Any(e => e.RecordsID == id)).GetValueOrDefault();
        }
    }
}
