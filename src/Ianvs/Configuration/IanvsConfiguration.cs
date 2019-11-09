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
    }
}