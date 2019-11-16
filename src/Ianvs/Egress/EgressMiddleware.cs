using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Dispatch;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Egress
{
    /// <summary>
    /// Egress Middleware is responsible for making the call to the backend service
    /// </summary>
    public class EgressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// initializes the egress middleware
        /// </summary>
        /// <param name="next">The next middleware in the processing chain</param>
        /// <param name="logger">The Ianvs logger</param>
        public EgressMiddleware(RequestDelegate next, ILogger<EgressMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IanvsContext ianvsContext, DispatcherFactory dispatcherFactory)
        {
            // TODO: Implement Protocol Translation - e.g. REST to gRPC
            // https://github.com/onyx-ws/ianvs/issues/11

            // Get the dispatcher matching for the backend
            ianvsContext.Dispatcher = dispatcherFactory.GetDispatcher(ianvsContext.MatchedEndpoint.Protocol);
            // Prepare backend request message
            ianvsContext.BackendMessage.Message = ianvsContext.Dispatcher.PrepareRequest(ianvsContext);

            Stopwatch backendTimer = new Stopwatch();
            backendTimer.Start();

            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Sending request to backend service {ianvsContext.MatchedOperation.OperationId} {ianvsContext.MatchedOperation.Method} {ianvsContext.MatchedEndpoint.Url} {ianvsContext.TargetServer.Url}");
            Task<object> backendResponse = ianvsContext.Dispatcher.Dispatch(ianvsContext);

            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Waiting for backend service response");
            backendResponse.Wait();

            backendTimer.Stop();

            ianvsContext.BackendResponse = new BackendMessage()
            {
                Message = backendResponse.Result
            };

            ianvsContext.Dispatcher.ProcessResponse(ianvsContext);

            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Backend service response received in {backendTimer.ElapsedMilliseconds}ms {ianvsContext.StatusCode}");

            // Egress middleware is the last in the Ianvs processing chain
            // No call is made to _next
        }
    }
}