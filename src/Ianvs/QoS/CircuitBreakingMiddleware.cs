using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.QoS
{
    public class CircuitBreakingMiddleware
    {
        private readonly RequestDelegate _next;

        public CircuitBreakingMiddleware(RequestDelegate next)
        {
            // TODO: Implement Circuit Breaking
            // https://github.com/onyx-ws/ianvs/issues/7
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
