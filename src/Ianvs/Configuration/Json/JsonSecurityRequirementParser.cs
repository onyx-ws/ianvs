using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class JsonSecurityRequirementParser
    {
        /// <summary>
        /// Parses a JSON representation of an Open API Security Requirement object
        /// </summary>
        /// <param name="serversData">JSON representation of an Open API Security Requirement object</param>
        /// <returns>An Open API Security Requirement object</returns>
        public static List<SecurityRequirement> Parse(JsonElement securityData)
        {
            List<SecurityRequirement> security = new List<SecurityRequirement>();
            foreach (JsonElement scheme in securityData.EnumerateArray())
            {
                security.Add(ParseSecurityRequirement(scheme));
            }
            return security;
        }

        private static SecurityRequirement ParseSecurityRequirement(JsonElement definition)
        {
            foreach (JsonProperty item in definition.EnumerateObject())
            {
                SecurityRequirement scheme = new SecurityRequirement()
                {
                    SchemeName = item.Name,
                    Scopes = new string[item.Value.GetArrayLength()]
                };

                int i = 0;
                foreach (JsonElement element in item.Value.EnumerateArray())
                {
                    scheme.Scopes[i++] = element.GetString();
                }
                return scheme;
            }
            return null;
        }
    }
}