using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.QoS
{
    public class ConcurrencyMiddleware
    {
        private readonly RequestDelegate _next;

        public ConcurrencyMiddleware(RequestDelegate next)
        {
            // TODO: Implement Concurrent calls
            // https://github.com/onyx-ws/ianvs/issues/12
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
