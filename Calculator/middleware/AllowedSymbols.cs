using System.Text;
using System.Text.RegularExpressions;
using Calculator.Controller;
using Calculator.middleware;
using Microsoft.Extensions.Options;

namespace Calculator.middleware
{
    // Класс проверяющий вводные данные с использованием регулярного выражения
    public class AllowedSymbols
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<CalculatorOptions> _options;

        public AllowedSymbols(RequestDelegate next, IOptions<CalculatorOptions> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string AllowedValues = _options.Value.AllowedOperators;
            
            // Регулярка, проверяющая что данные имеют формат некоторого математического выражения
            Regex symbolsAllowed = new Regex($"^([1-9][0-9]*[{AllowedValues}]{{1}}[1-9][0-9]*){{1}}([{AllowedValues}]{{1}}[1-9][0-9]*)*$");

            var expression = httpContext.Request.Query["expression"];

            if (symbolsAllowed.IsMatch(expression))

                await _next.Invoke(httpContext);

            else
            {
                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync("Bad Request");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AllowedSymbolsExtension
    {
        public static IApplicationBuilder UseSymbolsCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AllowedSymbols>();
        }
    }
}