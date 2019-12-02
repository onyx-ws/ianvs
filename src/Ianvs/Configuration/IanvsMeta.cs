using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration
{
    public static class IanvsMeta
    {
        public const string E_CONFIG_IANVS_VERSION = "x-ianvs";
        public static readonly string[] SUPPORTED_VERSIONS = new string[]  {
            "0.0.1"
        };

        public const string E_CONFIG_OPENAPI = "openapi";
        public static ImmutableDictionary<string, string[]> SUPPORTED_OA_VERSIONS =
            ImmutableDictionary.CreateRange<string, string[]>(new Dictionary<string, string[]>(){
                { "0.0.1", new string[] { "3.0.0", "3.0.1", "3.0.2" } }
            });

        public const string E_CONFIG_PATHS = "paths";

        public const string E_CONFIG_VIRTUAL_ENDPOINT = "x-ianvs-endpoint";
        public const string E_CONFIG_LOAD_BALANCER_METHOD = "x-ianvs-lb-method";
        
        public const string E_CONFIG_COMPONENTS = "components";

        public const string E_CONFIG_SECURITY_SCHEMES = "securitySchemes";
        public const string E_CONFIG_SEC_SCHEME_TYPE = "type";
        public const string E_CONFIG_SEC_SCHEME_IN = "in";
        public const string E_CONFIG_SEC_SCHEME_OPEN_ID_CONNECT_URL = "openIdConnectUrl";
        public const string E_CONFIG_SEC_SCHEME_ISSUER = "x-issuer";
        public const string E_CONFIG_SEC_SCHEME_AUDIENCES = "x-audience";

        public const string E_CONFIG_OPER_SECURITY = "security";
    }
}