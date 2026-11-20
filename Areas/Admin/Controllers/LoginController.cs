
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clubmanager.Areas.Admin.Models;
using clubmanager.Models;
using clubmanager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace aznews.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AdminUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName)
                || string.IsNullOrEmpty(user.Password))
            {
                return View();
            }

            // Convert password to MD5
            string pw = Functions.MD5Password(user.Password);

            // Check account in database
            var check = _context.AdminUsers
                .FirstOrDefault(u =>
                    u.UserName == user.UserName &&
                    u.Password == pw &&
                    u.IsActive == true);

            if (check == null)
            {
                Functions._AdminMessage = "Invalid username and password!";
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            Functions._AdminMessage = string.Empty;
            Functions._AdminID = check.UserID;
            Functions._AdminName = string.IsNullOrEmpty(check.UserName) ? string.Empty : check.UserName;
            Functions._AdminEmail = string.IsNullOrEmpty(check.Email) ? string.Empty : check.Email;

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
