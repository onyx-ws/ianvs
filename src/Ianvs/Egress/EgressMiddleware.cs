using Microsoft.AspNetCore.Http;
using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Egress
{
    public class EgressMiddleware
    {
        private readonly RequestDelegate _next;

        public EgressMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IanvsContext ianvsContext)
        {
            // TODO: Implement Egress operations
            dynamic backendResponse = await ianvsContext.Dispatcher.Dispatch(ianvsContext);

            ianvsContext.DownstreamResponse = new DownstreamMessage()
            {
                Message = backendResponse
            };
        }
    }
}