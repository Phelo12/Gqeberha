using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class GqebheraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
