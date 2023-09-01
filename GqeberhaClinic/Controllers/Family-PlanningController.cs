using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class Family_PlanningController : Controller
    {
 
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
