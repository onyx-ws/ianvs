using Ianvs = Onyx.Ianvs.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Onyx.Ianvs.Http
{
    public class IanvsContext
    {
        public Ianvs::Endpoint MatchedEndpoint { get; set; }
        public Ianvs::Operation MatchedOperation { get; set; }
        public Ianvs::Server TargetServer { get; set; }
    }
}