using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Ingress
{
    public class IngressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public IngressMiddleware(RequestDelegate next, ILogger<IngressMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IanvsContext ianvsContext)
        {
            // TODO: Implement Ingress operations
            // 1. Promote request variables
            // 2.? 
            Stopwatch ianvsTimer = new Stopwatch();
            ianvsTimer.Start();

            ianvsContext.ReceivedAt = DateTimeOffset.UtcNow;
            ianvsContext.RequestId = httpContext.TraceIdentifier;
            ianvsContext.Url = httpContext.Request.GetDisplayUrl();
            ianvsContext.Method = httpContext.Request.Method;
            ianvsContext.Protocol = httpContext.Request.Protocol;

            _logger.LogInformation($"Request {ianvsContext.RequestId} recieved at {ianvsContext.ReceivedAt}");

            if (httpContext.Request.Headers.TryGetValue("x-ianvs-trackid", out StringValues trackId))
            {
                ianvsContext.TrackId = trackId.ToString();
            }
            else
            {
                ianvsContext.TrackId = Guid.NewGuid().ToString();
            }

            ianvsContext.Variables = new Dictionary<string, string>();
            if (httpContext.Request.Body != null && httpContext.Request.Body.CanRead)
            {
                ianvsContext.IncomingRequest = await httpContext.Request.ReadAsStringAsync();
                ianvsContext.Variables.TryAdd("{request.body}", ianvsContext.IncomingRequest);
                ianvsContext.Variables.TryAdd("{request.header.content-Type}", httpContext.Request.ContentType);
                if (httpContext.Request.ContentLength.HasValue)
                {
                    ianvsContext.Variables.TryAdd("{request.header.content-Length}", httpContext.Request.ContentLength.Value.ToString());
                }
                else
                {
                    ianvsContext.Variables.TryAdd("{request.header.content-Length}", ianvsContext.IncomingRequest.Length.ToString());
                }
            }
            PromoteVariables(httpContext, ianvsContext);

            await _next(httpContext);

            httpContext.Response.OnCompleted(async () =>
            {
                ianvsTimer.Stop();
                ianvsContext.ProcessingCompletedAt = DateTimeOffset.UtcNow;
                _logger.LogInformation($"Response for {ianvsContext.RequestId} received by client at {ianvsContext.ProcessingCompletedAt}. Processing took {ianvsTimer.ElapsedMilliseconds}");
            });

            ianvsContext.ProcessingTime = ianvsTimer.ElapsedMilliseconds;
            ianvsContext.ResponseSentAt = DateTimeOffset.UtcNow;
            httpContext.Response.StatusCode = ianvsContext.StatusCode;
            _logger.LogInformation($"Response for {ianvsContext.RequestId} being sent at {ianvsContext.ResponseSentAt}. Processing took {ianvsContext.ProcessingTime}");

            await httpContext.Response.WriteAsync(ianvsContext.Response);
        }

        public void PromoteVariables(HttpContext httpContext, IanvsContext ianvsContext)
        {
            foreach (var header in httpContext.Request.Headers)
            {
                ianvsContext.Variables.TryAdd($"{{request.header.{header.Key}}}", header.Value.ToString());
            }
            foreach (var query in httpContext.Request.Query)
            {
                ianvsContext.Variables.TryAdd($"{{request.query.{query.Key}}}", query.Value.ToString());
            }
            ianvsContext.Variables.TryAdd("{request.received-at}", ianvsContext.ReceivedAt.ToUnixTimeMilliseconds().ToString());
            ianvsContext.Variables.TryAdd("{request.id}", ianvsContext.RequestId);
            ianvsContext.Variables.TryAdd("{request.track-id}", ianvsContext.TrackId);
            ianvsContext.Variables.TryAdd("{url}", ianvsContext.Url);
            ianvsContext.Variables.TryAdd("{method}", ianvsContext.Method);
            ianvsContext.Variables.TryAdd("{protocol}", ianvsContext.Protocol);
        }
    }
}