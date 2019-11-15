using Microsoft.AspNetCore.Http;
using Onyx.Ianvs.Dispatch;
using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.ProtocolTranslation
{
    public class ProtocolTranslationMiddleware
    {
        private readonly RequestDelegate _next;

        public ProtocolTranslationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IanvsContext ianvsContext)
        {
            await _next(context);
        }
    }
}