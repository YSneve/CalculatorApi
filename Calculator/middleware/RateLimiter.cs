using System.Text;
using Microsoft.Extensions.Options;

namespace Calculator.middleware
{
    public class RateLimiterOptions
    {
        public int Limit { get; set; }
    }
    // Класс ограничения количества обрабатываемых запросов использующий семафор
    public class RateLimiter
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly RequestDelegate _next;
        private readonly IOptions<RateLimiterOptions> _options;

        public RateLimiter(RequestDelegate next, IOptions<RateLimiterOptions> options)
        {
            _next = next;
            _options = options;
            _semaphore = new SemaphoreSlim(_options.Value.Limit);
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if (_semaphore.CurrentCount != 0)
            {
                _semaphore.WaitAsync();
                try
                {
                    await _next(httpContext); // calling next middleware
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            else
            {
                httpContext.Response.StatusCode = 503;
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync("Service Unavailable");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RateLimiterExtension
    {
        public static IApplicationBuilder UseRateLimiter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimiter>();
        }
    }
}
