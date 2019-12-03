using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class JsonSecuritySchemeParser
    {
        /// <summary>
        /// Parses a JSON representation of an Open API Security Schemes object
        /// </summary>
        /// <param name="serversData">JSON representation of an Open API Security Schemes object</param>
        /// <returns>An Open API Security Schemes object</returns>
        public static List<SecurityScheme> Parse(JsonElement securitySchemesData)
        {
            List<SecurityScheme> securitySchemes = new List<SecurityScheme>();
            foreach (JsonProperty scheme in securitySchemesData.EnumerateObject())
            {
                securitySchemes.Add(ParseSecurityScheme(scheme));
            }
            return securitySchemes;
        }

        private static SecurityScheme ParseSecurityScheme(JsonProperty definition)
        {
            SecurityScheme scheme = new SecurityScheme()
            {
                Name = definition.Name
            };
            foreach (JsonProperty property in definition.Value.EnumerateObject())
            {
                if (property.Name == IanvsMeta.E_CONFIG_SEC_SCHEME_TYPE) scheme.Type = property.Value.GetString();
                if (property.Name == IanvsMeta.E_CONFIG_SEC_SCHEME_IN) scheme.In = property.Value.GetString();
                if (property.Name == IanvsMeta.E_CONFIG_SEC_SCHEME_BEARER_FORMAT) scheme.BearerFormat = property.Value.GetString();
                if (property.Name == IanvsMeta.E_CONFIG_SEC_SCHEME_OPEN_ID_CONNECT_URL) scheme.OpenIdConnectUrl = property.Value.GetString();
                if (property.Name == IanvsMeta.E_CONFIG_SEC_SCHEME_ISSUER) scheme.Issuer = property.Value.GetString();
                if (property.Name == IanvsMeta.E_CONFIG_SEC_SCHEME_AUDIENCES)
                {
                    scheme.Audiences = new string[property.Value.GetArrayLength()];
                    for (int i = 0; i < scheme.Audiences.Length; i++)
                    {
                        scheme.Audiences[i] = property.Value[i].GetString();
                    }
                }
            }
            return scheme;
        }
    }
}