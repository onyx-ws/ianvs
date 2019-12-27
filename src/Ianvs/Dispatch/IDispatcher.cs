using Onyx.Ianvs.Common;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Dispatch
{
    public interface IDispatcher
    {
        Task<object> Dispatch(IanvsContext ianvsContext);
        object PrepareRequest(IanvsContext ianvsContext, ISpan egressSpan);
        void ProcessResponse(IanvsContext ianvsContext);
    }
}