using Onyx.Ianvs.Common;
using Onyx.Ianvs.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Dispatch
{
    /// <summary>
    /// Dispatches requests to backend services over HTTP protocol
    /// </summary>
    public class HttpDispatcher : IDispatcher
    {
        public async Task<object> Dispatch(IanvsContext ianvsContext)
        {
            try
            {
                using HttpClient client = new HttpClient(
                    new HttpClientHandler()
                    {
                        AllowAutoRedirect = false
                    }
                );
                string targetUrl = $"{ianvsContext.TargetUrl}?";
                foreach (var parameter in ianvsContext.MatchedOperation.Parameters)
                {
                    parameter.To ??= parameter.In;
                    if (parameter.To == "query")
                    {
                        if(ianvsContext.Variables.TryGetValue($"{{request.query.{parameter.Name}}}", out string value)){
                            targetUrl += $"{parameter.Name}={value}&";
                        }
                    }
                }
                // Remove last character - either ? or &
                targetUrl = targetUrl.Remove(targetUrl.Length - 1, 1);
                client.BaseAddress = new Uri(targetUrl);
                HttpRequestMessage downstreamMsg = ianvsContext.BackendMessage.Message;
                return await client.SendAsync(downstreamMsg);
            }
            catch
            {
                throw;
            }
        }

        public object PrepareRequest(IanvsContext ianvsContext)
        {
            HttpRequestMessage downstreamMsg = new HttpRequestMessage
            {
                Method = new HttpMethod(ianvsContext.MatchedOperation.Method)
            };
            return downstreamMsg;
        }

        public async void ProcessResponse(IanvsContext ianvsContext)
        {
            HttpResponseMessage backendHttpResponse = ianvsContext.BackendResponse.Message as HttpResponseMessage;
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
    }
}