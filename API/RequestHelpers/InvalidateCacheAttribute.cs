using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.RequestHelpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InvalidateCacheAttribute(params string[] patterns) : Attribute, IAsyncActionFilter
    {
        private readonly string[] _patterns = patterns;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (resultContext.Exception == null || resultContext.ExceptionHandled)
            {
                var cacheService = context.HttpContext.RequestServices
                    .GetRequiredService<IResponseCacheService>();

                foreach (var pattern in _patterns)
                {
                    await cacheService.RemoveCacheByPattern(pattern);
                }
            }
        }
    }
}
