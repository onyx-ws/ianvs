using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Configuration.Store;
using System;
using System.Threading.Tasks;

namespace Onyx.Ianvs.LoadBalancing
{
    /// <summary>
    /// Responsible for balancing load on backend servers
    /// </summary>
    public class LoadBalancingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public LoadBalancingMiddleware(RequestDelegate next, IServiceProvider serviceProvider, ILogger<LoadBalancingMiddleware> logger)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IanvsContext ianvsContext,
            IIanvsConfigurationStore ianvsConfiguration, LoadBalancerFactory loadBalancerFactory)
        {
            // TODO: Implement Load Balancing
            // WIP - https://github.com/onyx-ws/ianvs/issues/5

            // Support for different load balancing modes - Random/Round Robin/etc.
            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} finding load balancer");
            string loadBalancerMode = ianvsContext.MatchedOperation.LoadBalancerMethod;
            if (string.IsNullOrWhiteSpace(loadBalancerMode))
            {
                loadBalancerMode = ianvsContext.MatchedEndpoint.LoadBalancerMethod;
            }
            if (string.IsNullOrWhiteSpace(loadBalancerMode))
            {
                loadBalancerMode = ianvsConfiguration.LoadBalancerMethod;
            }

            ILoadBalancer loadBalancer = loadBalancerFactory.GetLoadBalancer(loadBalancerMode);
            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} {loadBalancerMode} load balancer found");

            // Operation level servers take priority
            if (!(ianvsContext.MatchedOperation.Servers?.Count == 0))
            {
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Selecting server out of {ianvsContext.MatchedOperation.Servers.Count} server(s)");
                ianvsContext.TargetServer = await loadBalancer.Next(ianvsContext.MatchedOperation.Servers);
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Server {ianvsContext.TargetServer.Url} selected");
            }
            // If not operation level servers then use Endpoint level servers
            else if (!(ianvsContext.MatchedEndpoint.Servers?.Count == 0))
            {
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Selecting server out of {ianvsContext.MatchedEndpoint.Servers.Count} server(s)");
                ianvsContext.TargetServer = await loadBalancer.Next(ianvsContext.MatchedEndpoint.Servers);
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Server {ianvsContext.TargetServer.Url} selected");
            }
            // Else use global servers defined
            else
            {
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Selecting server out of {ianvsConfiguration.Servers.Count} server(s)");
                ianvsContext.TargetServer = await loadBalancer.Next(ianvsConfiguration.Servers);
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Server {ianvsContext.TargetServer.Url} selected");
            }

            ianvsContext.TargetUrl = ianvsContext.TargetServer.Url + (ianvsContext.MatchedEndpoint.Url == "/" ? "" : ianvsContext.MatchedEndpoint.Url);

            await _next(context);
        }
    }
}