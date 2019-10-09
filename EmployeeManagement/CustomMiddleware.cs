using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public static class CustomMiddleware
    {
        public static IApplicationBuilder UseLoggingMiddleware(
                this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }

        public class LoggingMiddleware
        {
            private readonly RequestDelegate _next;
            private ILogger<LoggingMiddleware> logger;

            public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
            {
                _next = next;
                this.logger = logger;
                this.logger.LogWarning($"Warning1 : log from custom middleware");
            }

            // IMyScopedService is injected into Invoke
            public async Task Invoke(HttpContext httpContext)
            {
                await _next(httpContext);
            }
        }
    }
}
