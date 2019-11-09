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
    }
}