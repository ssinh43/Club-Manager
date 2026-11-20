using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using clubmanager.Models;
using clubmanager.Utilities;

namespace clubmanager.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (!Functions.IsLogin())
            return RedirectToAction("Index","Login");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

        public IActionResult Logout()
    {
        Functions._UserID = 0;
        Functions._UserName = string.Empty;
        Functions._Email = string.Empty;
        Functions._Message = string.Empty;
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
