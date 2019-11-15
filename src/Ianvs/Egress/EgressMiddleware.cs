using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Dispatch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Onyx.Ianvs.Egress
{
    public class EgressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public EgressMiddleware(RequestDelegate next, ILogger<EgressMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IanvsContext ianvsContext)
        {
            // TODO: Implement Protocol Translation - e.g. REST to gRPC
            // https://github.com/onyx-ws/ianvs/issues/11

            ianvsContext.Dispatcher = DispatcherFactory.CreateDispatcher(ianvsContext.MatchedEndpoint.Protocol);
            ianvsContext.BackendMessage.Message = ianvsContext.Dispatcher.PrepareRequest(ianvsContext);

            Stopwatch backendTimer = new Stopwatch();

            backendTimer.Start();

            Task<object> backendResponse = ianvsContext.Dispatcher.Dispatch(ianvsContext);

            backendResponse.Wait();

            backendTimer.Stop();

            _logger.LogInformation($"Backend operation '{ianvsContext.MatchedOperation.OperationId}' respnse received in {backendTimer.ElapsedMilliseconds} milliseconds");

            ianvsContext.BackendResponse = new BackendMessage()
            {
                Message = backendResponse.Result
            };

            ianvsContext.Dispatcher.ProcessResponse(ianvsContext);
        }
    }
}