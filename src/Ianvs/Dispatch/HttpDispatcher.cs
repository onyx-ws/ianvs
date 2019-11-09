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
                client.BaseAddress = new Uri(ianvsContext.DownstreamUrl);
                HttpRequestMessage downstreamMsg = ianvsContext.DownstreamRequest.Message;
                return await client.SendAsync(downstreamMsg);
            }
            catch
            {
                throw;
            }
        }

        public object PrepareRequest(IanvsContext ianvsContext)
        {
            HttpRequestMessage downstreamMsg = new HttpRequestMessage();
            downstreamMsg.Method = new HttpMethod(ianvsContext.MatchedOperation.Method);
            return downstreamMsg;
        }

        public async void ProcessResponse(IanvsContext ianvsContext)
        {
            string content = await (ianvsContext.DownstreamResponse.Message as HttpResponseMessage).ReadBodyAsStringAsync();
            ianvsContext.DownstreamResponse.Content = content;
        }
    }
}