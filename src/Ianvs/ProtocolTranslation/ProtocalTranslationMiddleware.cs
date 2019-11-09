using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.ProtocolTranslation
{
    public class ProtocalTranslationMiddleware
    {
        private readonly RequestDelegate _next;

        public ProtocalTranslationMiddleware(RequestDelegate next)
        {
            // TODO: Implement Protocal Translation - e.g. REST to gRPC
            // https://github.com/onyx-ws/ianvs/issues/11
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}