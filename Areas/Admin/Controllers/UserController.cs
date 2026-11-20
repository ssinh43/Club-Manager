using clubmanager.Areas.Admin.Models;
using clubmanager.Models;        // chứa DataContext (kết nối database)
using clubmanager.Utilities;     // chứa Functions (IsAdminLogin, MD5Password...)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // chứa Controller, IActionResult...

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

//----------------------------------------------------------------------------------------------
 // GET: hiện trang xác nhận xóa
        [HttpGet]
        public IActionResult AdminDelete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var user = _context.AdminUsers.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: thực sự xóa khi bấm nút Delete trong trang xác nhận
        [HttpPost]
        public IActionResult AdminDelete(int id)
        {
            // Không cho xóa tài khoản đang đăng nhập (bạn đang chỉ có 1 tài khoản Admin1!)
            if (id == Functions._AdminID)
                return RedirectToAction("AdminList");

            var delUser = _context.AdminUsers.Find(id);
            if (delUser == null) return NotFound();

            _context.AdminUsers.Remove(delUser);
            _context.SaveChanges();
            return RedirectToAction("AdminList");
        }

        [HttpGet]
        public IActionResult AdminCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminCreate(AdminUser user)
        {
            // Kiểm tra dữ liệu, lỗi sẽ hiện dưới ô nhập
            if (string.IsNullOrWhiteSpace(user.UserName))
                ModelState.AddModelError("UserName", "Vui lòng nhập Username.");
            else if (_context.AdminUsers.Any(u => u.UserName == user.UserName))
                ModelState.AddModelError("UserName", "Username đã tồn tại.");

            if (string.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", "Vui lòng nhập mật khẩu.");

            if (ModelState.IsValid)
            {
                user.Password = Functions.MD5Password(user.Password!);  // mã hóa trước khi lưu
                _context.AdminUsers.Add(user);
                _context.SaveChanges();
                return RedirectToAction("AdminList");
            }
            return View(user);   // có lỗi thì hiện lại form kèm dữ liệu đã nhập
        }

        [HttpGet]
        public IActionResult AdminEdit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var user = _context.AdminUsers.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult AdminEdit(AdminUser model)
        {
            var user = _context.AdminUsers.Find(model.UserID);   // lấy bản ghi cũ
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(model.UserName))
                ModelState.AddModelError("UserName", "Vui lòng nhập Username.");
            else if (_context.AdminUsers.Any(u => u.UserName == model.UserName && u.UserID != model.UserID))
                ModelState.AddModelError("UserName", "Username đã tồn tại.");

            if (ModelState.IsValid)
            {
                user.UserName = model.UserName!.Trim();
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                // Chỉ đổi mật khẩu khi người dùng có nhập mật khẩu mới
                if (!string.IsNullOrEmpty(model.Password))
                    user.Password = Functions.MD5Password(model.Password);

                _context.SaveChanges();
                return RedirectToAction("AdminList");
            }
            return View(model);
        }

//----------------------------------------------------------------------------------------------

        // URL: /Admin/User/UserList  => hiển thị danh sách bảng tblUser
        public IActionResult UserList()
        {
            if (!Functions.IsAdminLogin())
                return RedirectToAction("Index", "Login", new { area = "Admin" });

            // Lấy toàn bộ dòng của bảng tblUser, truyền sang view UserList.cshtml
            return View(_context.User.ToList());
        }

                // ================= NGƯỜI DÙNG (tblUser) =================

        [HttpGet]
        public IActionResult UserCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserCreate(tblUser user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                ModelState.AddModelError("UserName", "Vui lòng nhập Username.");
            else if (_context.User.Any(u => u.UserName == user.UserName))
                ModelState.AddModelError("UserName", "Username đã tồn tại.");

            if (string.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", "Vui lòng nhập mật khẩu.");

            if (ModelState.IsValid)
            {
                user.UserName = user.UserName!.Trim();
                user.Password = Functions.MD5Password(user.Password!);
                _context.User.Add(user);
                _context.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult UserEdit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var user = _context.User.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult UserEdit(tblUser model)
        {
            var user = _context.User.Find(model.UserID);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(model.UserName))
                ModelState.AddModelError("UserName", "Vui lòng nhập Username.");
            else if (_context.User.Any(u => u.UserName == model.UserName && u.UserID != model.UserID))
                ModelState.AddModelError("UserName", "Username đã tồn tại.");

            if (ModelState.IsValid)
            {
                user.UserName = model.UserName!.Trim();
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                if (!string.IsNullOrEmpty(model.Password))
                    user.Password = Functions.MD5Password(model.Password);

                _context.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UserDelete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var user = _context.User.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult UserDelete(int id)
        {
            var delUser = _context.User.Find(id);
            if (delUser == null) return NotFound();

            try
            {
                _context.User.Remove(delUser);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Không xóa được vì người dùng còn dữ liệu liên quan.";
            }
            return RedirectToAction("UserList");
        }

       
    }
}