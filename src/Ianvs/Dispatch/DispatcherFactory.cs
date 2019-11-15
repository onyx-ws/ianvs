using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Dispatch
{
    public class DispatcherFactory
    {
        public static IDispatcher CreateDispatcher(string protocol)
        {
            return protocol switch
            {
                "http" => new HttpDispatcher() as IDispatcher,
                _ => new HttpDispatcher() as IDispatcher
            };
        }
    }
}
