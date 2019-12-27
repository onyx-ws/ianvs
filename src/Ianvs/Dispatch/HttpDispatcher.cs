using Microsoft.Extensions.Logging;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Http;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Dispatch
{
    /// <summary>
    /// Dispatches requests to backend services over HTTP protocol
    /// </summary>
    public class HttpDispatcher : IDispatcher
    {
        private readonly ILogger _logger;

        public HttpDispatcher(ILogger<HttpDispatcher> logger)
        {
            _logger = logger;
        }

        public async Task<object> Dispatch(IanvsContext ianvsContext)
        {
            HttpRequestMessage downstreamMsg = ianvsContext.BackendMessage.Message;

            try
            {
                using HttpClient client = new HttpClient(
                    new HttpClientHandler()
                    {
                        AllowAutoRedirect = false
                    }
                );
                
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Sending request to {downstreamMsg.Method} {downstreamMsg.RequestUri.ToString()}");
                HttpResponseMessage backendResponse = await client.SendAsync(downstreamMsg);
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Received response from {downstreamMsg.Method} {downstreamMsg.RequestUri.ToString()}");
                
                return backendResponse;
            }
            catch (Exception e)
            {
                // Ok, something wrong happened - call the SWAT team
                _logger.LogError($"{Environment.MachineName} {ianvsContext.RequestId} Error occured sending request to {downstreamMsg.Method} {downstreamMsg.RequestUri.ToString()}");
                _logger.LogError($"{Environment.MachineName} {ianvsContext.RequestId} Error: {e.ToString()}");

                HttpResponseMessage error = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                return error;
            }
        }

        /// <summary>
        /// Prepare HTTP request message
        /// </summary>
        /// <param name="ianvsContext">Ianvs processing context</param>
        /// <returns>The HTTP request message to send to the backend</returns>
        public object PrepareRequest(IanvsContext ianvsContext, ISpan egressSpan)
        {
            string targetUrl = $"{ianvsContext.TargetUrl}?";
            foreach (var parameter in ianvsContext.MatchedOperation.Parameters)
            {
                parameter.To ??= parameter.In;
                if (parameter.To == "query")
                {
                    if (ianvsContext.Variables.TryGetValue($"{{request.query.{parameter.Name}}}", out string value))
                    {
                        targetUrl += $"{parameter.Name}={value}&";
                    }
                    else if (parameter.Default != null)
                    {
                        targetUrl += $"{parameter.Name}={parameter.Default}&";
                    }
                }
            }
            // Remove last character - either ? or &
            targetUrl = targetUrl.Remove(targetUrl.Length - 1, 1);

            HttpRequestMessage downstreamMsg = new HttpRequestMessage
            {
                Method = new HttpMethod(ianvsContext.MatchedOperation.Method),
                RequestUri = new Uri(targetUrl)
            };
            if (!string.IsNullOrWhiteSpace(ianvsContext.IncomingRequest))
            {
                downstreamMsg.Content = new StringContent(
                    ianvsContext.IncomingRequest,
                    Encoding.UTF8,
                    ianvsContext.Variables.GetValueOrDefault("{request.header.content-Type}", "application/json")
                );
            }
            if (egressSpan.Context.IsValid)
            {
                ianvsContext.Tracer.TextFormat.Inject(
                    egressSpan.Context,
                    downstreamMsg.Headers,
                    (headers, name, value) => headers.Add(name, value));
            }
            return downstreamMsg;
        }

        /// <summary>
        /// Process backend HTTP response message into common Ianvs data
        /// </summary>
        /// <param name="ianvsContext">The Ianvs processing context</param>
        public async void ProcessResponse(IanvsContext ianvsContext)
        {
            HttpResponseMessage backendHttpResponse = ianvsContext.BackendResponse.Message as HttpResponseMessage;
            ianvsContext.StatusCode = (int)backendHttpResponse.StatusCode;

            if (backendHttpResponse.Content != null)
            {
                ianvsContext.BackendResponse.Content =
                    await backendHttpResponse.ReadBodyAsStringAsync();

                ianvsContext.BackendResponse.Headers =
                    backendHttpResponse.Headers.ToDictionary(
                        header => header.Key,
                        header => header.Value
                    )
                    .Concat(
                        backendHttpResponse.Content.Headers.ToDictionary(
                            header => header.Key,
                            header => header.Value
                        ))
                    .ToDictionary(
                        header => header.Key,
                        header => header.Value
                    );
            }
            else
            {
                ianvsContext.BackendResponse.Content = string.Empty;

                ianvsContext.BackendResponse.Headers =
                        backendHttpResponse.Headers.ToDictionary(
                            header => header.Key,
                            header => header.Value
                        );
            }
        }
    }
}