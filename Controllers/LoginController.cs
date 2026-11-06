using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clubmanager.Models;
using clubmanager.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace clubmanager.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        // GET: /Login
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
        [HttpPost]
        public IActionResult Index(tblUser user)
        {
            if (user == null) return NotFound();
            
            // Convert password to MD5
            string pw = Functions.MD5Password(user.Password);
            
            // Check the information in database
            var check = _context.User.Where(u => (u.UserName == user.UserName) && (u.Password == pw)).FirstOrDefault();
            
            if (check == null)
            {
                Functions._Message = "Invalid username and password!";
                return RedirectToAction("Index", "Login");
            }
            
            Functions._Message = string.Empty;
            Functions._UserID = check.UserID ?? 0;
            Functions._UserName = string.IsNullOrEmpty(check.UserName) ? string.Empty : check.UserName;
            Functions._Email = string.IsNullOrEmpty(check.Email) ? string.Empty : check.Email;
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Functions._UserID = 0;
            Functions._UserName = string.Empty;
            Functions._Email = string.Empty;
            Functions._Message = string.Empty;
            return RedirectToAction("Index", "Home");
        }
    }
}