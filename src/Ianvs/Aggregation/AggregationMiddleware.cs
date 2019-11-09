using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Aggregation
{
    public class AggregationMiddleware
    {
        private readonly RequestDelegate _next;

        public AggregationMiddleware(RequestDelegate next)
        {
            // TODO: Implement Aggregation 
            // https://github.com/onyx-ws/ianvs/issues/12
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
