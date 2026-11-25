using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace clubmanager.Areas.Admin.Filters;
public sealed class ClubAdminSessionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Session.GetInt32("ClubAdminId") == null)
            context.Result = new RedirectToActionResult("Index", "Login", new { area = "Admin" });
    }
}
