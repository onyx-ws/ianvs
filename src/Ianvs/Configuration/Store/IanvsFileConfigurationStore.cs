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

        public IanvsFileConfigurationStore()
        {
            string data = File.ReadAllText("./ianvs.json");
            ianvsConfiguration = IanvsJsonConfigurationParser.Parse(data);
        }
    }
}