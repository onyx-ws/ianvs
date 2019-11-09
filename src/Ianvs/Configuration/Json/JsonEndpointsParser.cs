using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class JsonEndpointsParser
    {
        public static List<Endpoint> Parse(JsonElement jEndpoints)
        {
            List<Endpoint> endpoints = new List<Endpoint>();

            foreach (JsonProperty endpointData in jEndpoints.EnumerateObject())
            {
                endpoints.Add(Parse(endpointData));
            }

            return endpoints;
        }

        private static Endpoint Parse(JsonProperty endpointData)
        {
            Endpoint endpoint = new Endpoint
            {
                Url = endpointData.Name,
                Operations = new List<Operation>()
            };

            foreach (JsonProperty property in endpointData.Value.EnumerateObject())
            {
                if (property.Name == IanvsMeta.E_CONFIG_VIRTUAL_ENDPOINT) endpoint.IanvsUrl = property.Value.GetString();
                if (property.Name == "get") endpoint.Operations.Add(JsonOperationParser.Parse(property));
                if (property.Name == "put") endpoint.Operations.Add(JsonOperationParser.Parse(property));
                if (property.Name == "post") endpoint.Operations.Add(JsonOperationParser.Parse(property));
                if (property.Name == "delete") endpoint.Operations.Add(JsonOperationParser.Parse(property));
                if (property.Name == "options") endpoint.Operations.Add(JsonOperationParser.Parse(property));
                if (property.Name == "head") endpoint.Operations.Add(JsonOperationParser.Parse(property));
                if (property.Name == "patch") endpoint.Operations.Add(JsonOperationParser.Parse(property));
                if (property.Name == "trace") endpoint.Operations.Add(JsonOperationParser.Parse(property));
            }

            return endpoint;
        }
    }
}
