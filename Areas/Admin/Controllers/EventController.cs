using clubmanager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clubmanager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly DataContext _context;

        public EventController(DataContext context)
        {
            _context = context;
        }

        // GET: /Admin/Event hoặc /Admin/Event/Index
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return View(events);
        }

        // GET: /Admin/Event/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Event
            {
                EventDate = DateTime.Now
            });
        }

        // POST: /Admin/Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Events.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm sự kiện thành công.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Event/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Events.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: /Admin/Event/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = await _context.Events.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.EventName = model.EventName;
            item.EventDate = model.EventDate;
            item.Location = model.Location;
            item.Description = model.Description;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật sự kiện thành công.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Event/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Events.FindAsync(id);
            if (item != null)
            {
                _context.Events.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa sự kiện.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}