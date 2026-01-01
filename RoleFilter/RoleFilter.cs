using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Test_App.RoleFilter
{
    public class RoleFilter : IActionFilter
    {
        private readonly string _role;
        public RoleFilter(string role) => _role = role;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (role != _role)
                context.Result = new UnauthorizedResult();
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
