using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class IanvsJsonConfigurationParser
    {
        /// <summary>
        /// Parses Ianvs JSON config data
        /// </summary>
        /// <param name="data">Ianvs JSON config data</param>
        /// <returns>Ianvs configuration data</returns>
        public static IanvsConfiguration Parse(string data)
        {
            IanvsConfiguration ianvs = new IanvsConfiguration();

            using var jDocument = JsonDocument.Parse(data);
            var jIanvs = jDocument.RootElement;

            if (jIanvs.TryGetProperty(IanvsMeta.E_CONFIG_IANVS_VERSION, out JsonElement jVersion))
            {
                string ianvsVersion = jVersion.GetString();
                if (IanvsMeta.SUPPORTED_VERSIONS.Contains(ianvsVersion))
                {
                    ianvs.Version = ianvsVersion;
                }
                else
                {
                    // TODO: handle unsupported Ianvs version
                    // https://github.com/onyx-ws/ianvs/issues/4
                }
            }
            else
            {
                // TODO: handle JSON parse errors
                // https://github.com/onyx-ws/ianvs/issues/3
            }

            if (jIanvs.TryGetProperty(IanvsMeta.E_CONFIG_PATHS, out JsonElement jPaths))
            {
                ianvs.Endpoints = JsonEndpointsParser.Parse(jPaths);
            }
            else
            {
                // TODO: handle JSON parse errors
                // https://github.com/onyx-ws/ianvs/issues/3
            }

            if (jIanvs.TryGetProperty(IanvsMeta.E_CONFIG_LOAD_BALANCER_METHOD, out JsonElement jLoadBalancerMethod))
            {
                ianvs.LoadBalancerMethod = jLoadBalancerMethod.ToString();
            }

            if (jIanvs.TryGetProperty(IanvsMeta.E_CONFIG_COMPONENTS, out JsonElement components))
            {
                if(components.TryGetProperty(IanvsMeta.E_CONFIG_SECURITY_SCHEMES, out JsonElement securitySchemes))
                {
                    ianvs.SecuritySchemes = JsonSecuritySchemeParser.Parse(securitySchemes);
                }
            }

            return ianvs;
        }
    }
}