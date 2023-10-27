using GqeberhaClinic.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class Family_PlanningController : Controller
    {

        private readonly ApplicationDbContext _context;

        public Family_PlanningController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Patient()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            ViewBag.His = _context.ContraceptivePrescription.Include(c => c.Patient).Where(a => a.PatientId == user).ToList();
            return View();
        }
            public IActionResult Nurse()
            {
            ViewBag.Pre = _context.Prescription.Include(p => p.Doctor).Include(p => p.Patient).ToList();
            return View();
            }
        }
}
