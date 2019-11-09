using Ianvs = Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Onyx.Ianvs.Dispatch;

namespace Onyx.Ianvs.Common
{
    public class IanvsContext
    {
        public Ianvs::Endpoint MatchedEndpoint { get; internal set; }
        public Ianvs::Operation MatchedOperation { get; internal set; }
        public Ianvs::Server TargetServer { get; internal set; }
        public DateTimeOffset ReceivedAt { get; internal set; }
        public Dictionary<string, string> Variables { get; internal set; }
        public string IncomingRequest { get; internal set; }
        public string DownstreamUrl { get; internal set; }
        public IDispatcher Dispatcher { get; internal set; }
        public DownstreamMessage DownstreamRequest { get; internal set; }
        public DownstreamMessage DownstreamResponse { get; internal set; }
        public string Response { get; internal set; }
    }
}