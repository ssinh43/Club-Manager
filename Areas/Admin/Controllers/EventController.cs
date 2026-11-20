using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ClubManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private const string SessionKey = "AdminEventList";

        public class EventItemViewModel
        {
            public int Id { get; set; }
            public string EventName { get; set; } = string.Empty;
            public DateTime? EventDate { get; set; }
            public string? Location { get; set; }
            public string? Description { get; set; }
        }

        private List<EventItemViewModel> GetEvents()
        {
            var json = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json))
            {
                return new List<EventItemViewModel>();
            }
            return JsonSerializer.Deserialize<List<EventItemViewModel>>(json) ?? new List<EventItemViewModel>();
        }

        private void SaveEvents(List<EventItemViewModel> events)
        {
            var json = JsonSerializer.Serialize(events);
            HttpContext.Session.SetString(SessionKey, json);
        }

        // Danh sách sự kiện (/Admin/Event/Index)
        public IActionResult Index()
        {
            var events = GetEvents();
            return View(events);
        }

        // Thêm mới sự kiện - mở form (/Admin/Event/Create)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Thêm mới sự kiện - xử lý lưu (/Admin/Event/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string eventName, DateTime? eventDate, string location, string? description)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                ViewBag.Error = "Vui lòng nhập tên sự kiện.";
                return View();
            }

            var events = GetEvents();
            var nextId = events.Count > 0 ? events.Max(x => x.Id) + 1 : 1;

            events.Add(new EventItemViewModel
            {
                Id = nextId,
                EventName = eventName.Trim(),
                EventDate = eventDate,
                Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim(),
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim()
            });

            SaveEvents(events);
            TempData["Success"] = $"Đã thêm sự kiện: {eventName.Trim()}";
            return RedirectToAction(nameof(Index));
        }

        // Sửa sự kiện - mở form (/Admin/Event/Edit/1)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var events = GetEvents();
            var item = events.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                TempData["Error"] = "Không tìm thấy sự kiện cần sửa.";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        // Sửa sự kiện - xử lý cập nhật (/Admin/Event/Edit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string eventName, DateTime? eventDate, string location, string? description)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                ViewBag.Error = "Vui lòng nhập tên sự kiện.";
                return View(new EventItemViewModel
                {
                    Id = id,
                    EventName = eventName ?? string.Empty,
                    EventDate = eventDate,
                    Location = location,
                    Description = description
                });
            }

            var events = GetEvents();
            var item = events.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                TempData["Error"] = "Không tìm thấy sự kiện cần sửa.";
                return RedirectToAction(nameof(Index));
            }

            item.EventName = eventName.Trim();
            item.EventDate = eventDate;
            item.Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
            item.Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

            SaveEvents(events);
            TempData["Success"] = $"Đã cập nhật sự kiện: {item.EventName}";
            return RedirectToAction(nameof(Index));
        }

        // Xóa sự kiện (/Admin/Event/Delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var events = GetEvents();
            var item = events.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                TempData["Error"] = "Không tìm thấy sự kiện cần xóa.";
                return RedirectToAction(nameof(Index));
            }

            var deletedName = item.EventName;
            events.Remove(item);
            SaveEvents(events);

            TempData["Success"] = $"Đã xóa sự kiện: {deletedName}";
            return RedirectToAction(nameof(Index));
        }
    }
}