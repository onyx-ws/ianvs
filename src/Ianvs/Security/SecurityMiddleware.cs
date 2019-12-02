using Ianvs = Onyx.Ianvs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.Ianvs.Configuration.Store;

namespace Onyx.Ianvs.Security
{
    /// <summary>
    /// Responsible for applied authentication and authorization policies on incoming requests
    /// </summary>
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, Ianvs::IanvsContext ianvsContext,
            IIanvsConfigurationStore ianvsConfiguration, AuthenticatorFactory authenticatorFactory)
        {
            // TODO: Implement Security
            // https://github.com/onyx-ws/ianvs/issues/8

            // Any security requirements?
            ianvsContext.Security = GetSecurityRequirements(ianvsContext);
            // Yes
            if (ianvsContext.Security?.Count > 0)
            {
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Authenticating request");
                AuthenticationResult authResult = await Authenticate(httpContext, ianvsContext, ianvsConfiguration, authenticatorFactory);
                if (!authResult.Authenticated)
                {
                    _logger.LogWarning($"{Environment.MachineName} {ianvsContext.RequestId} Request authentication failed");
                    _logger.LogWarning($"{Environment.MachineName} {ianvsContext.RequestId} Authentication Error: {authResult.Error}");
                    SetUnAuthorizedResponse(ianvsContext);
                    return;
                }
                else
                {
                    _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Request authenticated successfully");
                    ianvsContext.Principal = authResult.Principal;

                    _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Authorizing request");
                    authResult = await Authorize(httpContext, ianvsContext);
                    if (!authResult.Authenticated)
                    {
                        _logger.LogWarning($"{Environment.MachineName} {ianvsContext.RequestId} Request authorization failed");
                        _logger.LogWarning($"{Environment.MachineName} {ianvsContext.RequestId} Authorization Error: {authResult.Error}");
                        SetForbiddenResponse(ianvsContext);
                        return;
                    }
                    else
                    {
                        _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Request authorized successfully");
                    }
                }
            }
            // request authenticated or no security requirement
            await _next(httpContext);
        }

        private List<Common.SecurityRequirement> GetSecurityRequirements(Common.IanvsContext ianvsContext)
        {
            return ianvsContext.MatchedOperation.Security ?? ianvsContext.MatchedEndpoint.Security ?? null;
        }

        private async Task<AuthenticationResult> Authenticate(HttpContext httpContext, Common.IanvsContext ianvsContext,
            IIanvsConfigurationStore ianvsConfiguration, AuthenticatorFactory authenticatorFactory)
        {
            // If multiple schemes are defined on the operation, only one can apply to the request; check which one
            foreach (Ianvs::SecurityRequirement securityRequirement in ianvsContext.Security)
            {
                Ianvs::SecurityScheme schemeDefinition = ianvsConfiguration.SecuritySchemes?
                                                            .Find(s => s.Name == securityRequirement.SchemeName);
                if (schemeDefinition != null)
                {
                    ianvsContext.SecurityScheme = schemeDefinition;
                    IAuthenticationHandler authenticator = authenticatorFactory.GetAuthenticator(ianvsContext.SecurityScheme);
                    if (authenticator.CanAuthenticate(httpContext, ianvsContext))
                    {
                        ianvsContext.SecurityRequirement = securityRequirement;
                        return await authenticator.Authenticate(httpContext, ianvsContext);
                    }
                    ianvsContext.SecurityScheme = null;
                }
            }

            // Couldn't apply security requirements
            return new AuthenticationResult()
            {
                Authenticated = false,
                Error = "No Matching Security Scheme"
            };
        }

        private async Task<AuthenticationResult> Authorize(HttpContext httpContext, Common.IanvsContext ianvsContext)
        {
            if (ianvsContext.SecurityRequirement?.Scopes?.Length > 0)
            {
                if (ianvsContext.Principal == null)
                {
                    return new AuthenticationResult()
                    {
                        Authenticated = false,
                        Error = "Missing Principal"
                    };
                }

                string[] grantedScopes = ianvsContext.Principal.Claims?.First(c => c.Type == "scope")?.Value.Split(' ') ?? new string[] { };
                if (grantedScopes.Length == 0)
                {
                    return new AuthenticationResult()
                    {
                        Authenticated = false,
                        Error = "No Scopes Granted"
                    };
                }

                List<string> missingScopes = ianvsContext.SecurityRequirement.Scopes.ToList()
                                                .Except(grantedScopes.ToList())
                                                .ToList();

                if (missingScopes?.Count > 0)
                {
                    return new AuthenticationResult()
                    {
                        Authenticated = false,
                        Error = "Missing Scopes"
                    };
                }
            }

            return new AuthenticationResult()
            {
                Authenticated = true
            };
        }

        private void SetUnAuthorizedResponse(Ianvs::IanvsContext ianvsContext)
        {
            // Client not authenticated
            // Return 401 - Unauthorized (https://tools.ietf.org/html/rfc7235#section-3.1)
            ianvsContext.StatusCode = 401;
            ianvsContext.Response = "";
            // TODO: Add WWW-Authenticate header
        }

        private void SetForbiddenResponse(Ianvs::IanvsContext ianvsContext)
        {
            // Client not authorized
            // Return 403 - Forbidden (https://tools.ietf.org/html/rfc7231#section-6.5.3)
            ianvsContext.StatusCode = 403;
            ianvsContext.Response = "";
        }
    }
}