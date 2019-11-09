using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Monitoring
{
    public class MonitoringMiddleware
    {
        private readonly RequestDelegate _next;

        public MonitoringMiddleware(RequestDelegate next)
        {
            // TODO: Implement Monitoring
            // https://github.com/onyx-ws/ianvs/issues/6
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}