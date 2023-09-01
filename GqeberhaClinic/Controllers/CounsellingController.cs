using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class CounsellingController : Controller
    {
      
        public IActionResult Patient()
        {
            return View();
        } 
        public IActionResult Doctor()
        {
            return View();
        }
        public IActionResult Consellor()
        {
            return View();
        }
    }
}
