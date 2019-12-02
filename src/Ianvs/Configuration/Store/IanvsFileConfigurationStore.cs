using Onyx.Ianvs.Common;
using Onyx.Ianvs.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Store
{
    public class IanvsFileConfigurationStore : IIanvsConfigurationStore
    {
        private readonly IanvsConfiguration ianvsConfiguration;

        /// <summary>
        /// Holds Ianvs configuration schema version
        /// </summary>
        public string Version
        {
            get { return ianvsConfiguration.Version; }
            set { }
        }

        /// <summary>
        /// List of service endpoints configured
        /// </summary>
        public List<Endpoint> Endpoints
        {
            get { return ianvsConfiguration.Endpoints; }
            set { }
        }

        /// <summary>
        /// List of backend servers configured
        /// </summary>
        public List<Server> Servers
        {
            get { return ianvsConfiguration.Servers; }
            set { }
        }

        /// <summary>
        /// The default load balancer mode used to select the server to service operations. Default is random
        /// </summary>
        public string LoadBalancerMethod
        {
            get { return ianvsConfiguration.LoadBalancerMethod; }
            set { }
        }

        /// <summary>
        /// The list of defined security schemes
        /// </summary>
        public List<SecurityScheme> SecuritySchemes
        {
            get { return ianvsConfiguration.SecuritySchemes; }
            set { }
        }

        public IanvsFileConfigurationStore()
        {
            string data = File.ReadAllText("./ianvs.json");
            ianvsConfiguration = IanvsJsonConfigurationParser.Parse(data);
        }
    }
}