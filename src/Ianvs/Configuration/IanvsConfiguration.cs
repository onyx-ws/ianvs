using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration
{
    public class IanvsConfiguration
    {
        /// <summary>
        /// Holds Ianvs configuration schema version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// List of service endpoints configured
        /// </summary>
        public List<Endpoint> Endpoints { get; set; }

        /// <summary>
        /// List of backend servers configured
        /// </summary>
        public List<Server> Servers { get; set; }

        /// <summary>
        /// The default load balancer mode used to select the server to service operations. Default is random
        /// </summary>
        public string LoadBalancerMethod { get; set; } = "random";
    }
}