using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class JsonOperationParser
    {
        /// <summary>
        /// Parses a JSON representation of an Open API Operation object
        /// </summary>
        /// <param name="operationData">JSON representation of an Open API Operation object</param>
        /// <returns>Describes a single API operation on a path</returns>
        public static Operation Parse(JsonProperty operationData)
        {
            Operation operation = new Operation
            {
                Method = operationData.Name.ToUpperInvariant()
            };

            foreach (JsonProperty property in operationData.Value.EnumerateObject())
            {
                if (property.Name == "tags") operation.Tags = JsonTagsParser.Parse(property.Value);
                if (property.Name == "summary") operation.Summary = property.Value.GetString();
                if (property.Name == "description") operation.Description = property.Value.GetString();
                if (property.Name == "operationId") operation.OperationId = property.Value.GetString();
                if (property.Name == "servers") operation.Servers = JsonServerParser.Parse(property.Value);
                if (property.Name == IanvsMeta.E_CONFIG_LOAD_BALANCER_METHOD) operation.LoadBalancerMethod = property.Value.ToString();
                if (property.Name == "parameters") operation.Parameters = JsonParameterParser.Parse(property.Value);
            }
            return operation;
        }
    }
}