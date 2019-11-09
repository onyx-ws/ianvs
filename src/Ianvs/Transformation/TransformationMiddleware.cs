using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Transformation
{
    public class TransformationMiddleware
    {
        private readonly RequestDelegate _next;

        public TransformationMiddleware(RequestDelegate next)
        {
            // TODO: Implement Transformation
            // https://github.com/onyx-ws/ianvs/issues/10
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
