using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.LoadBalancing
{
    public class LoadBalancingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoadBalancingMiddleware(RequestDelegate next)
        {
            // TODO: Implement Load Balancing
            // https://github.com/onyx-ws/ianvs/issues/5
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
