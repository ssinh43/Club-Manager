using clubmanager.Areas.Admin.Models;
using clubmanager.Models;
using clubmanager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace clubmanager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DataContext _context;
        public LoginController(DataContext context) { _context = context; }

        [HttpGet]
        public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(AdminUser user)
    {
        if (user == null) return NotFound();
        
        // Không dùng Functions.MD5Password nữa, lấy thẳng giá trị người dùng nhập
        string pw = user.Password; 

        var check = _context.AdminUsers
            .Where(u => u.UserName == user.UserName && u.Password == pw && u.IsActive == true)
            .FirstOrDefault();

        if (check == null)
        {
            Functions._AdminMessage = "Sai tên đăng nhập hoặc mật khẩu!";
            return RedirectToAction("Index", "Login");
        }

        Functions._AdminMessage = string.Empty;
        Functions._AdminID = check.UserID;
        Functions._AdminName = check.UserName ?? string.Empty;
        Functions._AdminEmail = check.Email ?? string.Empty;
        return RedirectToAction("Index", "Home");
    }
    }
}