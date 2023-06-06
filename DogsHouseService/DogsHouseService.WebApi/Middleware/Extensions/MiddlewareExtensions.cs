using DogsHouseService.WebApi.Middleware.CustomExceptionHandler;
using DogsHouseService.WebApi.Middleware.RateLimiting;

namespace DogsHouseService.WebApi.Middleware.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}
