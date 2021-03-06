﻿using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Store
{
    public interface IIanvsConfigurationStore
    {
        /// <summary>
        /// Holds Ianvs configuration schema version
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// List of service endpoints configured
        /// </summary>
        List<Endpoint> Endpoints { get; set; }

        /// <summary>
        /// List of backend servers configured
        /// </summary>
        List<Server> Servers { get; set; }

        /// <summary>
        /// The default load balancer mode used to select the server to service operations. Default is random
        /// </summary>
        public string LoadBalancerMethod { get; set; }

        /// <summary>
        /// The list of defined security schemes
        /// </summary>
        public List<SecurityScheme> SecuritySchemes { get; set; }
    }
}