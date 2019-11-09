using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class JsonTagsParser
    {
        /// <summary>
        /// Parses a JSON representation of an Open API Tags object
        /// </summary>
        /// <param name="operationData">JSON representation of an Open API Tags object</param>
        /// <returns>Describes A list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier</returns>
        public static List<string> Parse(JsonElement json)
        {
            return json.EnumerateArray().Select(s => s.GetString()).ToList<string>();
        }
    }
}