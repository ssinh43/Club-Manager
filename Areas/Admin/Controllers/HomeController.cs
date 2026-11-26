using clubmanager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace clubmanager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("ClubAdminId") == null)
                return RedirectToAction("Index", "Login", new { area = "Admin" });

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("ClubAdminId");
            Functions._AdminID = 0;
            Functions._AdminName = string.Empty;
            Functions._AdminEmail = string.Empty;
            Functions._AdminMessage = string.Empty;

            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }
    }
}