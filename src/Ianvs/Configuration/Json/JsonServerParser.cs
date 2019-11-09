using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration.Json
{
    public class JsonServerParser
    {
        /// <summary>
        /// Parses a JSON representation of an Open API Operation object
        /// </summary>
        /// <param name="operationData">JSON representation of an Open API Operation object</param>
        /// <returns>Describes a single API operation on a path</returns>
        public static List<Server> Parse(JsonElement serversData)
        {
            return null;
        }
    }
}
