using Onyx.Ianvs.Common;
using Onyx.Ianvs.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Security
{
    public class AuthenticatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthenticatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAuthenticationHandler GetAuthenticator(SecurityScheme scheme)
        {
            return scheme.Type.ToLowerInvariant() switch
            {
                "jwt" => (IAuthenticationHandler)_serviceProvider.GetService(typeof(JwtAuthenticationHandler)),
                _ => (IAuthenticationHandler)_serviceProvider.GetService(typeof(JwtAuthenticationHandler))
            };
        }
    }
}
