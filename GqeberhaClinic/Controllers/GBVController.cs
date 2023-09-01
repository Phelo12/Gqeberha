using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GqeberhaClinic.Controllers
{
    public class GBVController : Controller
    {
        [Authorize]

        public IActionResult Patient()
        {
            return View();
        }  
        public IActionResult Doctor()
        {
            return View();
        }
        public IActionResult Nurse()
        {
            return View();
        }
    }
}
