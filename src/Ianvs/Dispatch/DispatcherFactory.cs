using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Dispatch
{
    public class DispatcherFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DispatcherFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDispatcher GetDispatcher(string protocol)
        {
            return protocol switch
            {
                "http" => _serviceProvider.GetService(typeof(HttpDispatcher)) as IDispatcher,
                _ => _serviceProvider.GetService(typeof(HttpDispatcher)) as IDispatcher
            };
        }
    }
}
