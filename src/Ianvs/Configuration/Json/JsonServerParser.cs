using Onyx.Ianvs.Common;
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
        /// Parses a JSON representation of an Open API Servers object
        /// </summary>
        /// <param name="serversData">JSON representation of an Open API Servers object</param>
        /// <returns>An Open API Servers object</returns>
        public static List<Server> Parse(JsonElement serversData)
        {
            List<Server> servers = new List<Server>();
            foreach (JsonElement server in serversData.EnumerateArray())
            {
                servers.Add(ParseServer(server));
            }
            return servers;
        }

        private static Server ParseServer(JsonElement definition)
        {
            Server server = new Server();
            foreach (JsonProperty property in definition.EnumerateObject())
            {
                if (property.Name == "url") server.Url = property.Value.GetString();
                if (property.Name == "description") server.Description = property.Value.GetString();
            }
            return server;
        }
    }
}