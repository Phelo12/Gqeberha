using Microsoft.AspNetCore.Mvc;

namespace GqeberhaClinic.Controllers
{
    public class CounsellingSessions : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
