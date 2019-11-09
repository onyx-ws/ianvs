using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Common
{
    public class DownstreamMessage
    {
        public string Content { get; internal set; }
        public Dictionary<string, string> Headers { get; internal set; }
        public dynamic Message { get; internal set; }
    }
}
