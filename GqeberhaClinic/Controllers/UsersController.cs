using GqeberhaClinic.Areas.Identity.Data;
using GqeberhaClinic.Data;
using GqeberhaClinic.Models;
using GqeberhaClinic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GqeberhaClinic.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender emailSender;

        private readonly UserManager<GqebheraUser> _userManager;
        public UsersController(ApplicationDbContext context, IEmailSender email, UserManager<GqebheraUser> userManager)
        {
            _context = context;
            emailSender = email;
            
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var Userlist = _context.Users.ToList();
            ViewBag.List = Userlist;
            return View();
        }
        public async Task<IActionResult> Add_New_User()
        {
            ViewBag.Roles = _context.Roles.ToList();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add_New_User(UserVM user)
        {
            if (ModelState.IsValid)
            {
                var ourUser = new GqebheraUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = true,
                    UserName = user.Email,
                };
                string Passwords = "Gqebhera" + user.FirstName + user.Email + "2023@";
                var result = await _userManager.CreateAsync(ourUser, Passwords);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(ourUser, user.Role);
                    await emailSender.SendEmailAsync(ourUser.Email, "Welcome to Gqebhera",
                        $"<html> <head> <style> body {{ font-family: Arial, sans-serif; }} " +
                        $"h1 {{ color: #336699; }}" +
                        $".cta-button {{ background-color: #336699; color: white;" +
                        $" padding: 10px 20px;" +
                        $" text-decoration: none; border-radius: 5px; }}" +
                        $".cta-button:hover {{ background-color: #265580; }}" +
                        $".footer {{ margin-top: 20px; font-size: 12px; color: #888; }}" +
                        $"  </style>" +
                        $"</head>" +
                        $"<body>" +
                        $"" +
                        $"<h1>Welcome to Gqebhera!</h1>" +
                        $"<p>Dear {user.FirstName} {user.LastName},</p>" +
                        $"<p>Pleas note, your account has been create by our admini, please feel freee to access and we happy to have you on board</p>" +
                        $"<h4>Below is your Credentials</h4>" +
                        $"<p>Email Address: {ourUser.Email}</p>" +
                        $"<p>Password: {Passwords}</p>" +
                        $"<p>If you have any questions or need assistance, please don't hesitate to contact our support team at info@GqebheraClinic.com.</p>" +
                        $"<div class='footer'>" +
                        $" <p>Thank you,</p>" +
                        $" <p>Gqebhera clinic Team</p>" +
                        $"</div>" +
                        $" </body>" +
                        $"</html>");
                    var loginUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var alert = new Alert()
                    {
                        IntendedUser = loginUser,
                        Message = "New User has been created",
                        Role = "Notification",

                    };
                    _context.Alerts.Add(alert);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "New User has been added and activate successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

    }
}
