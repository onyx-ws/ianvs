using Microsoft.AspNetCore.Http;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Ingress
{
    public class IngressMiddleware
    {
        private readonly RequestDelegate _next;

        public IngressMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IanvsContext ianvsContext)
        {
            // TODO: Implement Ingress operations
            // 1. Promote request variables
            // 2.? 

            ianvsContext.ReceivedAt = DateTimeOffset.UtcNow;
            ianvsContext.Variables = new Dictionary<string, string>();
            if (httpContext.Request.Body != null && httpContext.Request.Body.CanRead)
            {
                ianvsContext.IncomingRequest = await httpContext.Request.ReadAsStringAsync();
                ianvsContext.Variables.TryAdd("{request.body}", ianvsContext.IncomingRequest);
                ianvsContext.Variables.TryAdd("{request.contentType}", httpContext.Request.ContentType);
                if (httpContext.Request.ContentLength.HasValue)
                {
                    ianvsContext.Variables.TryAdd("{request.contentLength}", httpContext.Request.ContentLength.Value.ToString());
                }
                else
                {
                    ianvsContext.Variables.TryAdd("{request.contentLength}", ianvsContext.IncomingRequest.Length.ToString());
                }
            }

            await _next(httpContext);

            await httpContext.Response.WriteAsync(ianvsContext.Response);
        }
    }
}