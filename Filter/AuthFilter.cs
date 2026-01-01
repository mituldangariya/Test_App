using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Test_App.Filter
{
    public class AuthFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetInt32("UserId") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
