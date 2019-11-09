using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Filtering
{
    public class FilteringMiddleware
    {
        private readonly RequestDelegate _next;

        public FilteringMiddleware(RequestDelegate next)
        {
            // TODO: Implement Filtering (Blacklist/Whitelist)
            // https://github.com/onyx-ws/ianvs/issues/13
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
