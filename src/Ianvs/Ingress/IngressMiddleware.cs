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
            try
            {
                // TODO: Implement Ingress operations
                Stopwatch ianvsTimer = new Stopwatch();
                ianvsTimer.Start();

                ianvsContext.ReceivedAt = DateTimeOffset.UtcNow;
                ianvsContext.RequestId = httpContext.TraceIdentifier;
                ianvsContext.Url = httpContext.Request.GetDisplayUrl();
                ianvsContext.Method = httpContext.Request.Method;
                ianvsContext.Protocol = httpContext.Request.Protocol;

                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Request starting {ianvsContext.Protocol} {ianvsContext.Method} {ianvsContext.Url}");
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Request recieved at {ianvsContext.ReceivedAt}");

                if (httpContext.Request.Headers.TryGetValue("x-ianvs-trackid", out StringValues trackId))
                {
                    ianvsContext.TrackId = trackId.ToString();
                }
                else
                {
                    ianvsContext.TrackId = Guid.NewGuid().ToString();
                }

                PromoteVariables(httpContext, ianvsContext);

                // When response is starting capture and log time Ianvs took to process
                httpContext.Response.OnStarting(async () =>
                {
                    ianvsContext.ProcessingTime = ianvsTimer.ElapsedMilliseconds;
                    ianvsContext.ResponseSentAt = DateTimeOffset.UtcNow;
                    _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Response being sent at {ianvsContext.ResponseSentAt}.");
                    _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Processing took {ianvsContext.ProcessingTime}ms");
                });

                // When response is completed (i.e. Received by client) capture and log time it took
                httpContext.Response.OnCompleted(async () =>
                {
                    ianvsTimer.Stop();
                    ianvsContext.ProcessingCompletedAt = DateTimeOffset.UtcNow;
                    _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Response received by client at {ianvsContext.ProcessingCompletedAt}");
                    _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Request finished in {ianvsTimer.ElapsedMilliseconds}ms {httpContext.Response.StatusCode}");
                });
                
                await _next(httpContext);

                httpContext.Response.StatusCode = ianvsContext.StatusCode;
                await httpContext.Response.WriteAsync(ianvsContext.Response);
            }
            catch(Exception e)
            {
                // Ok, something wrong happened - call the SWAT team
                _logger.LogError($"{Environment.MachineName} {ianvsContext.RequestId} Error occured processing request");
                _logger.LogError($"{Environment.MachineName} {ianvsContext.RequestId} Error: {e.ToString()}");
                
                ianvsContext.ResponseSentAt = DateTimeOffset.UtcNow;
                ianvsContext.StatusCode = 500;
                httpContext.Response.StatusCode = ianvsContext.StatusCode;

                await httpContext.Response.WriteAsync(string.Empty);
            }
        }

        /// <summary>
        /// Promote request context variables to be used in variable swap operations
        /// </summary>
        /// <param name="httpContext">The incoming HTTP context</param>
        /// <param name="ianvsContext">The Ianvs processing context</param>
        public void PromoteVariables(HttpContext httpContext, IanvsContext ianvsContext)
        {
            ianvsContext.Variables = new Dictionary<string, string>();
            if (httpContext.Request.Body != null && httpContext.Request.Body.CanRead)
            {
                ianvsContext.IncomingRequest = httpContext.Request.ReadAsStringAsync().Result;
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