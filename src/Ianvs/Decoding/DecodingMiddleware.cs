using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Decoding
{
    public class DecodingMiddleware
    {
        private readonly RequestDelegate _next;

        public DecodingMiddleware(RequestDelegate next)
        {
            // TODO: Implement Decode/Encode
            // https://github.com/onyx-ws/ianvs/issues/14
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
