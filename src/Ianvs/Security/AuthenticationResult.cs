using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Security
{
    public class AuthenticationResult
    {
        /// <summary>
        /// Indicates if the request have been authenticated or not
        /// </summary>
        public bool Authenticated { get; set; } = false;

        /// <summary>
        /// The error message returned by the authentication provider
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The authenticated principal information
        /// </summary>
        public ClaimsPrincipal Principal { get; set; }
    }
}