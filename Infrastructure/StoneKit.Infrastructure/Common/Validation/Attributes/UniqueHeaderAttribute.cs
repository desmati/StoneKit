using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

using System.Net;

namespace FluentValidation
{
    public sealed class UniqueHeaderAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Request.Headers.TryGetValue("unique", out StringValues value);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new UserFriendlyException("Unique http header is missing.");
            }

            await base.OnActionExecutionAsync(context, next);

            context.HttpContext.Response?.Headers?.TryAdd("unique", value);
        }
    }
}
