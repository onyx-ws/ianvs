using Onyx.Ianvs.Common;
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
    }
}