using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.LoadBalancing
{
    public class LoadBalancerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public LoadBalancerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILoadBalancer GetLoadBalancer(string method)
        {
            return method switch
            {
                "random" => (ILoadBalancer)_serviceProvider.GetService(typeof(RandomLoadBalancer)),
                _ => (ILoadBalancer)_serviceProvider.GetService(typeof(RandomLoadBalancer))
            };
        }
    }
}
