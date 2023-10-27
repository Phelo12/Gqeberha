using GqeberhaClinic.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class Walk_InController : Controller
    {
        private readonly ApplicationDbContext _context;
        public Walk_InController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Patient()
        {

            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.Appointmnet =  _context.Appointments.Include(a => a.Patient).Where(e => e.PatientID == user).OrderByDescending(a => a.CreatedDate).ToList();
            try
            {
                var File = _context.Medical_File.Where(a => a.PatientID == user).FirstOrDefault();
                if (File != null)
                {
                    TempData["FileFound"] = "Found";
                }
            }
            catch(Exception ex)
            {

            }

            return View();
        }  public IActionResult Nurse()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;

            ViewBag.appoint = _context.Appointments.Include(a => a.Patient).OrderByDescending(a => a.CreatedDate).ToList();
            return View();
        }  public IActionResult Admin()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            return View();
        }
    }
}
