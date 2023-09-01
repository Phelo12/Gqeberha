using GqeberhaClinic.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class ChronicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _email;
        public ChronicController(ApplicationDbContext context, IEmailSender email)
        {
            _context = context;
            _email = email;
        }

        public IActionResult Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderBy(a => a.Date).ToList();
            ViewBag.Alert = alert;
            return View();
        }
        public IActionResult Patient()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderBy(a => a.Date).ToList();
            ViewBag.Alert = alert;
            return View();
        }
        public IActionResult Nurse()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderBy(a => a.Date).ToList();
            ViewBag.Alert = alert;
            return View();
        }
    }
}
