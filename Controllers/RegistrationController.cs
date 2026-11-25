using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using clubmanager.Models;
using clubmanager.Utilities;

namespace clubmanager.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly DataContext _context;

        public RegistrationController(DataContext context)
        {
            _context = context;
        }

        // Đăng ký sự kiện
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventID)
        {
            int userID = Functions._UserID;

            if (userID <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(x => x.EventID == eventID && x.IsActive);

            if (ev == null)
            {
                TempData["Message"] = "Sự kiện không tồn tại!";
                return RedirectToAction("Index", "Event");
            }

            if (ev.StartDate <= DateTime.Now)
            {
                TempData["Message"] = "Sự kiện đã bắt đầu hoặc đã kết thúc!";
                return RedirectToAction("Index", "Event");
            }

            var oldRegistration = await _context.EventRegistrations
                .FirstOrDefaultAsync(x =>
                    x.EventID == eventID &&
                    x.UserID == userID);

            if (oldRegistration != null &&
                oldRegistration.Status == "Đã đăng ký")
            {
                TempData["Message"] = "Bạn đã đăng ký sự kiện này rồi!";

                return RedirectToAction("Details", "Event",
                    new { id = eventID });
            }

            int registeredCount = await _context.EventRegistrations
                .CountAsync(x =>
                    x.EventID == eventID &&
                    x.Status == "Đã đăng ký");

            if (ev.MaxParticipants.HasValue &&
                registeredCount >= ev.MaxParticipants.Value)
            {
                TempData["Message"] = "Sự kiện đã đủ số lượng người tham gia!";

                return RedirectToAction("Details", "Event",
                    new { id = eventID });
            }

            if (oldRegistration != null)
            {
                oldRegistration.Status = "Đã đăng ký";
                oldRegistration.RegistrationDate = DateTime.Now;
            }
            else
            {
                var registration = new tblEventRegistration
                {
                    EventID = eventID,
                    UserID = userID,
                    RegistrationDate = DateTime.Now,
                    Status = "Đã đăng ký"
                };

                _context.EventRegistrations.Add(registration);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Đăng ký sự kiện thành công!";

            return RedirectToAction("My");
        }

        // Danh sách sự kiện người dùng đã đăng ký
        public async Task<IActionResult> My()
        {
            int userID = Functions._UserID;

            if (userID <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var registrations = await (
                from r in _context.EventRegistrations
                join e in _context.Events
                    on r.EventID equals e.EventID
                where r.UserID == userID
                select new RegistrationViewModel
                {
                    RegistrationID = r.RegistrationID,
                    EventID = r.EventID,
                    EventName = e.EventName,
                    StartDate = e.StartDate,
                    Location = e.Location,
                    RegistrationDate = r.RegistrationDate,
                    Status = r.Status
                }
            ).ToListAsync();

            return View(registrations);
        }

        // Hủy đăng ký sự kiện
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            int userID = Functions._UserID;

            if (userID <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var registration = await _context.EventRegistrations
                .FirstOrDefaultAsync(x =>
                    x.RegistrationID == id &&
                    x.UserID == userID &&
                    x.Status == "Đã đăng ký");

            if (registration == null)
            {
                return NotFound();
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(x => x.EventID == registration.EventID);

            if (ev == null || ev.StartDate <= DateTime.Now)
            {
                TempData["Message"] =
                    "Không thể hủy đăng ký vì sự kiện đã bắt đầu!";

                return RedirectToAction("My");
            }

            registration.Status = "Đã hủy";

            await _context.SaveChangesAsync();

            TempData["Message"] = "Hủy đăng ký thành công!";

            return RedirectToAction("My");
        }
    }
}
