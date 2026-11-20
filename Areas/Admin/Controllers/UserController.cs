using clubmanager.Models;        // chứa DataContext (kết nối database)
using clubmanager.Utilities;     // chứa Functions (IsAdminLogin, MD5Password...)
using Microsoft.AspNetCore.Mvc;  // chứa Controller, IActionResult...

namespace clubmanager.Areas.Admin.Controllers
{
    [Area("Admin")]  // báo cho MVC biết controller này thuộc Area "Admin" => URL: /Admin/User/...
    public class UserController : Controller
    {
        // DataContext là "cầu nối" tới database (Entity Framework)
        private readonly DataContext _context;

        // ASP.NET tự truyền DataContext vào đây (Dependency Injection)
        public UserController(DataContext context) => _context = context;

        // URL: /Admin/User/AdminList  => hiển thị danh sách bảng AdminUser
        public IActionResult AdminList()
        {
            // Chưa đăng nhập thì đá về trang Login, tránh người lạ xem được danh sách
            if (!Functions.IsAdminLogin())
                return RedirectToAction("Index", "Login", new { area = "Admin" });

            // Lấy toàn bộ dòng của bảng AdminUser, truyền sang view AdminList.cshtml
            return View(_context.AdminUsers.ToList());
        }

        // URL: /Admin/User/UserList  => hiển thị danh sách bảng tblUser
        public IActionResult UserList()
        {
            if (!Functions.IsAdminLogin())
                return RedirectToAction("Index", "Login", new { area = "Admin" });

            // Lấy toàn bộ dòng của bảng tblUser, truyền sang view UserList.cshtml
            return View(_context.User.ToList());
        }
    }
}