using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagerApp.Utils
{
    public class AuthorizeUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
            }
            base.OnActionExecuting(context);
        }

    }
}
