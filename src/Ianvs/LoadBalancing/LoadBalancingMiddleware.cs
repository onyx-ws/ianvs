using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Configuration.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.LoadBalancing
{
    public class LoadBalancingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public LoadBalancingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context, IanvsContext ianvsContext,
            IIanvsConfigurationStore ianvsConfiguration)
        {
            // TODO: Implement Load Balancing
            // WIP - https://github.com/onyx-ws/ianvs/issues/5

            // TODO: Implement support for different load balancing modes - Random/Round Robin/etc.
            ILoadBalancer loadBalancer = GetLoadBalancer("random");
            
            // Operation level servers take priority
            if(!(ianvsContext.MatchedOperation.Servers?.Count == 0))
            {
                ianvsContext.TargetServer = await loadBalancer.Next(ianvsContext.MatchedOperation.Servers);
            }
            // If not operation level servers then use Endpoint level servers
            else if (!(ianvsContext.MatchedEndpoint.Servers?.Count == 0))
            {
                ianvsContext.TargetServer = await loadBalancer.Next(ianvsContext.MatchedEndpoint.Servers);
            }
            // Else use global servers defined
            else
            {
                ianvsContext.TargetServer = await loadBalancer.Next(ianvsConfiguration.Servers);
            }

            ianvsContext.TargetUrl = ianvsContext.TargetServer.Url + ianvsContext.MatchedEndpoint.Url;

            await _next(context);
        }

        private ILoadBalancer GetLoadBalancer(string mode)
        {
            return mode switch
            {
                "random" => (ILoadBalancer)_serviceProvider.GetService(typeof(RandomLoadBalancer)),
                _ => (ILoadBalancer)_serviceProvider.GetService(typeof(RandomLoadBalancer))
            };
        }
    }
}