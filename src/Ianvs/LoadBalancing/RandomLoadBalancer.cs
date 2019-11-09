using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.Ianvs.Common;

namespace Onyx.Ianvs.LoadBalancing
{
    /// <summary>
    /// Selects the server to route traffic to randomly
    /// </summary>
    public class RandomLoadBalancer : ILoadBalancer
    {
        /// <summary>
        /// Returns the next server to route traffic to
        /// </summary>
        /// <param name="servers">The list of servers available</param>
        /// <returns>The next server to route traffic to</returns>
        public Task<Server> Next(List<Server> servers)
        {
            if(servers?.Count == 0)
            {
                return Task.FromResult<Server>(null);
            }

            if(servers.Count == 1)
            {
                return Task.FromResult<Server>(servers[0]);
            }

            int serverNumber = new Random().Next(1, servers.Count);
            return Task.FromResult<Server>(servers[serverNumber - 1]);
        }
    }
}
