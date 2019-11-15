using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class JsonParameterParser
    {
        /// <summary>
        /// Parses a JSON representation of an Open API Parameters object
        /// </summary>
        /// <param name="parametersData">JSON representation of an Open API Parameters object</param>
        /// <returns>An Open API Parameters object</returns>
        public static List<Parameter> Parse(JsonElement parametersData)
        {
            List<Parameter> parameters = new List<Parameter>();
            foreach (JsonElement parameter in parametersData.EnumerateArray())
            {
                parameters.Add(ParseParameter(parameter));
            }
            return parameters;
        }

        private static Parameter ParseParameter(JsonElement definition)
        {
            Parameter paramters = new Parameter();
            foreach (JsonProperty property in definition.EnumerateObject())
            {
                if (property.Name == "name") paramters.Name = property.Value.GetString();
                if (property.Name == "in") paramters.In = property.Value.GetString();
                if (property.Name == "x-to") paramters.To = property.Value.GetString();
                if (property.Name == "description") paramters.Description = property.Value.GetString();
                if (property.Name == "x-default") paramters.Default = property.Value.GetString();
            }
            return paramters;
        }
    }
}