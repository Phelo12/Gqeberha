using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class ChronicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Patient()
        {
            return View();
        }
        public IActionResult Nurse()
        {
            return View();
        }
    }
}
