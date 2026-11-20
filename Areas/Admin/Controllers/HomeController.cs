using clubmanager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace clubmanager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!Functions.IsAdminLogin()) return RedirectToAction("Index", "Login");
            return View();
        }

        public IActionResult Logout()
        {
            Functions._AdminID = 0;
            Functions._AdminName = string.Empty;
            Functions._AdminEmail = string.Empty;
            Functions._AdminMessage = string.Empty;
            return RedirectToAction("Index", "Login");
        }
    }
}