using GqeberhaClinic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GqeberhaClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult test()
        {
            return View();
        }
        public IActionResult GetProducts()
        {
            
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}