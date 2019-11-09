using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Dispatch
{
    public interface IDispatcher
    {
        Task<object> Dispatch(IanvsContext ianvsContext);
        object PrepareRequest(IanvsContext ianvsContext);
        void ProcessResponse(IanvsContext ianvsContext);
    }
}