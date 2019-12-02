using Microsoft.AspNetCore.Http;
using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Security
{
    /// <summary>
    /// Interface for standard Ianvs authentication handler
    /// </summary>
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// Accepts HTTP and Ianvs request contexts and performs Authentication required
        /// </summary>
        /// <param name="httpContext">The incoming request HTTP context</param>
        /// <param name="ianvsContext">The incoming request Ianvs context</param>
        /// <returns>The Ianvs Authentication result</returns>
        Task<AuthenticationResult> Authenticate(HttpContext httpContext, IanvsContext ianvsContext);

        /// <summary>
        /// Accepts HTTP and Ianvs request contexts and identifies if the request has matching authentication data
        /// </summary>
        /// <param name="httpContext">The incoming request HTTP context</param>
        /// <param name="ianvsContext">The incoming request Ianvs context</param>
        /// <returns>A flag indicating whether the authentication handler can authenticae the request</returns>
        bool CanAuthenticate(HttpContext httpContext, IanvsContext ianvsContext);
    }
}
