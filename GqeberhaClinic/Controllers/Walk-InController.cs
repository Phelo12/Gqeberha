using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class Walk_InController : Controller
    {
        public IActionResult Patient()
        {
            return View();
        }  public IActionResult Nurse()
        {
            return View();
        }  public IActionResult Admin()
        {
            return View();
        }
    }
}
