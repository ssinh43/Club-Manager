using clubmanager.Models;
using clubmanager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace clubmanager.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DataContext _context;

        public RegisterController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(tblUser auser)
        {
            if (auser == null) return NotFound();
            //Check the duplicate username before registering
            var check = _context.User.Where(u => (u.UserName == auser.UserName)).FirstOrDefault();
            if (check != null) //Already exists this username
            {
                Functions._Message = "Username already exists";
                return RedirectToAction("Index", "Register");
            }
            //If username does not exist
            Functions._Message = string.Empty;
            auser.Password = Functions.MD5Password(auser.Password);
            _context.User.Add(auser);
            _context.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}