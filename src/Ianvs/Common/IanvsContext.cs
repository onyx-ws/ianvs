using Ianvs = Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Onyx.Ianvs.Dispatch;
using System.Security.Claims;
using OpenTelemetry.Trace;

namespace Onyx.Ianvs.Common
{
    public class IanvsContext
    {
        public string RequestId { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Protocol { get; set; }
        public DateTimeOffset ReceivedAt { get; internal set; }
        public Ianvs::Endpoint MatchedEndpoint { get; internal set; }
        public Ianvs::Operation MatchedOperation { get; internal set; }
        public Ianvs::Server TargetServer { get; internal set; }
        public Dictionary<string, string> Variables { get; internal set; }
        public string IncomingRequest { get; internal set; }
        public string TargetUrl { get; internal set; }
        public IDispatcher Dispatcher { get; internal set; }
        public BackendMessage BackendMessage { get; internal set; }
        public BackendMessage BackendResponse { get; internal set; }
        public string Response { get; internal set; }
        public DateTimeOffset ResponseSentAt { get; internal set; }
        public DateTimeOffset ProcessingCompletedAt { get; internal set; }
        public int StatusCode { get; internal set; } = 200;
        public string TrackId { get; set; }
        public long ProcessingTime { get; internal set; }
        public SecurityScheme SecurityScheme { get; internal set; }
        public List<SecurityRequirement> Security { get; set; }
        public SecurityRequirement SecurityRequirement { get; internal set; }
        public ClaimsPrincipal Principal { get; internal set; }
        public ISpan TraceSpan { get; set; }
        public Tracer Tracer { get; set; }
    }
}