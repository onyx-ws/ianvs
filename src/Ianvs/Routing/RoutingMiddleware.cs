using Microsoft.AspNetCore.Http;
using Ianvs = Onyx.Ianvs.Configuration;
using Onyx.Ianvs.Configuration.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.Ianvs.Http;

namespace Onyx.Ianvs.Routing
{
    public class RoutingMiddleware
    {
        private readonly RequestDelegate _next;

        public RoutingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IanvsContext ianvsContext,
            IIanvsConfigurationStore ianvsConfiguration)
        {
            // TODO: Implement Routing
            // WIP - https://github.com/onyx-ws/ianvs/issues/1
            // TODO: Handle endpoints with templates
            // https://github.com/onyx-ws/ianvs/issues/2

            // Find a matching Path
            string requestPath = context.Request.Path;
            Ianvs::Endpoint matchedEndpoint = ianvsConfiguration.Endpoints.FirstOrDefault(
                endpoint =>
                    requestPath == (!string.IsNullOrWhiteSpace(endpoint.IanvsUrl) ? endpoint.IanvsUrl : endpoint.Url)
            );
            if (matchedEndpoint is null)
            {
                // Path not found
                // Return 404 - Not Found (https://tools.ietf.org/html/rfc7231#section-6.5.4)
                await context.Response.WriteNotFoundResponseAsync();
            }
            else
            {
                ianvsContext.MatchedEndpoint = matchedEndpoint;
                // Path found - Match Operation
                string requestMethod = context.Request.Method;
                Ianvs::Operation matchedOperation = matchedEndpoint.Operations.FirstOrDefault(
                    operation => 
                        operation.Method.Equals(requestMethod, StringComparison.InvariantCultureIgnoreCase)
                );
                if (matchedOperation is null)
                {
                    // Operation not found
                    // Return 405 - Method Not Allowed (https://tools.ietf.org/html/rfc7231#section-6.5.5)
                    await context.Response.WriteMethodNotAllowedResponseAsync();
                }
                else
                {
                    // Operation found
                    // Simulate operation
                    ianvsContext.MatchedOperation = matchedOperation;

                    await _next(context);
                    await context.Response.WriteAsync("This is Ianvs!");
                }
            }
        }
    }
}