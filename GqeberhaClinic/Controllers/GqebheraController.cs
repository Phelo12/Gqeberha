using GqeberhaClinic.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class GqebheraController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GqebheraController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = _context.Alerts.Where(a => a.IntendedUser == user).OrderByDescending(a => a.Date).ToList();
            ViewBag.Alert = alert;
            return View();
        }
    }
}
