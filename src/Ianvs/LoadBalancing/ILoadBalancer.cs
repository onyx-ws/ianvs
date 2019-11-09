using Ianvs = Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.LoadBalancing
{
    public interface ILoadBalancer
    {
        Task<Ianvs::Server> Next(List<Ianvs::Server> servers);
    }
}
