using Microsoft.AspNetCore.Http;
using Ianvs = Onyx.Ianvs.Common;
using Onyx.Ianvs.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.Ianvs.Configuration.Store;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

namespace Onyx.Ianvs.Routing
{
    /// <summary>
    /// Responsible for matching incoming request to a defined backend API
    /// </summary>
    public class RoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RoutingMiddleware(RequestDelegate next, ILogger<RoutingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, Ianvs::IanvsContext ianvsContext,
            IIanvsConfigurationStore ianvsConfiguration, Tracer tracer)
        {
            // TODO: Implement Routing
            // WIP - https://github.com/onyx-ws/ianvs/issues/1
            // TODO: Handle endpoints with templates
            // https://github.com/onyx-ws/ianvs/issues/2

            var routingSpan = tracer.StartSpan("ianvs-routing");

            // Find a matching Path
            string requestPath = httpContext.Request.Path;
            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Looking for a configured route");
            Ianvs::Endpoint matchedEndpoint = ianvsConfiguration.Endpoints.FirstOrDefault(
                endpoint =>
                    requestPath == (!string.IsNullOrWhiteSpace(endpoint.IanvsUrl) ? endpoint.IanvsUrl : endpoint.Url)
            );
            if (matchedEndpoint is null)
            {
                // Path not found
                // Return 404 - Not Found (https://tools.ietf.org/html/rfc7231#section-6.5.4)
                routingSpan.AddEvent("Route not matched");
                _logger.LogWarning($"{Environment.MachineName} {ianvsContext.RequestId} No configured route found");
                ianvsContext.StatusCode = 404;
                ianvsContext.Response = "";
            }
            else
            {
                ianvsContext.MatchedEndpoint = matchedEndpoint;
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Possible matching route found at {ianvsContext.MatchedEndpoint.Url}");

                // Path found - Match Operation
                string requestMethod = httpContext.Request.Method;
                Ianvs::Operation matchedOperation = matchedEndpoint.Operations.FirstOrDefault(
                    operation =>
                        operation.Method.Equals(requestMethod, StringComparison.InvariantCultureIgnoreCase)
                );
                if (matchedOperation is null)
                {
                    // Operation not found
                    // Return 405 - Method Not Allowed (https://tools.ietf.org/html/rfc7231#section-6.5.5)
                    routingSpan.AddEvent("Method not matched");
                    _logger.LogWarning($"{Environment.MachineName} {ianvsContext.RequestId} No configured operation found");
                    ianvsContext.StatusCode = 405;
                    ianvsContext.Response = "";
                }
                else
                {
                    // Operation found
                    // Simulate operation
                    routingSpan.AddEvent("Route found");

                    ianvsContext.MatchedOperation = matchedOperation;
                    _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Matched route at {ianvsContext.MatchedOperation.OperationId} {ianvsContext.MatchedOperation.Method} {ianvsContext.MatchedEndpoint.Url}");

                    await _next(httpContext);
                }
            }

            routingSpan.End();
        }
    }
}