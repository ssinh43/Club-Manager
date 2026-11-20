using clubmanager.Areas.Admin.Models;
using clubmanager.Models;
using Microsoft.AspNetCore.Mvc;

namespace clubmanager.Areas.Admin.Components
{
    [ViewComponent(Name = "AdminMenu")]
    public class AdminMenuComponent : ViewComponent
    {
        private readonly DataContext _context;
        public AdminMenuComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = (from m in _context.AdminMenus
                         where m.IsActive == true
                         orderby m.ItemOrder
                         select m).ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", items));
        }
    }
}