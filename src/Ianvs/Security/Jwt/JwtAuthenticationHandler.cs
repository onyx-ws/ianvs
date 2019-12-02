using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Onyx.Ianvs.Common;

namespace Onyx.Ianvs.Security.Jwt
{
    /// <summary>
    /// Validates request JWT tokens
    /// </summary>
    public class JwtAuthenticationHandler : IAuthenticationHandler
    {
        private readonly ILogger _logger;

        public JwtAuthenticationHandler(ILogger<JwtAuthenticationHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Authenticates the incoming request using JWT
        /// </summary>
        /// <param name="httpContext">The incoming request HTTP context</param>
        /// <param name="ianvsContext">The incoming request Ianvs context</param>
        /// <returns>The Ianvs Authentication result</returns>
        public async Task<AuthenticationResult> Authenticate(HttpContext httpContext, IanvsContext ianvsContext)
        {
            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Starting JWT Validation");

            // Get token value, only header and cookie are supported
            string tokenValue = GetTokenValue(httpContext, ianvsContext);
            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                _logger.LogError($"{Environment.MachineName} {ianvsContext.RequestId} No token found");

                return new AuthenticationResult()
                {
                    Authenticated = false,
                    Error = "Token missing"
                };
            }

            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Preparing validation parameters");

            // Get issuer configuration values
            OpenIdConnectConfiguration issureConfig = await GetIssuerConfig(ianvsContext);

            TokenValidationParameters validationParameters = GetValidationParameters(issureConfig, ianvsContext);

            ClaimsPrincipal principal;
            try
            {
                _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Validating request token");
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                principal = handler.ValidateToken(tokenValue, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception e)
            {
                _logger.LogError($"{Environment.MachineName} {ianvsContext.RequestId} Token validation failed. {e.Message}");

                return new AuthenticationResult()
                {
                    Authenticated = false,
                    Error = e.Message
                };
            }

            _logger.LogInformation($"{Environment.MachineName} {ianvsContext.RequestId} Token validated successfully");

            return new AuthenticationResult()
            {
                Authenticated = true,
                Principal = principal
            };
        }

        private static TokenValidationParameters GetValidationParameters(OpenIdConnectConfiguration issureConfig, IanvsContext ianvsContext)
        {
            return new TokenValidationParameters
            {
                ValidIssuer = issureConfig.Issuer,
                ValidAudiences = ianvsContext.SecurityScheme.Audiences,
                IssuerSigningKeys = issureConfig.SigningKeys
            };
        }

        /// <summary>
        /// Gets the OpenId config for the authentication authority
        /// </summary>
        /// <param name="ianvsContext">The incoming request Ianvs processing context</param>
        /// <returns>The OpenId config for the authentication authority</returns>
        private static async Task<OpenIdConnectConfiguration> GetIssuerConfig(IanvsContext ianvsContext)
        {
            OpenIdConnectConfiguration openIdConfig = new OpenIdConnectConfiguration();
            if (!string.IsNullOrWhiteSpace(ianvsContext.SecurityScheme.OpenIdConnectUrl))
            {
                IConfigurationManager<OpenIdConnectConfiguration> configurationManager =
                    new ConfigurationManager<OpenIdConnectConfiguration>(
                        ianvsContext.SecurityScheme.OpenIdConnectUrl,
                        new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever() { RequireHttps = false }
                    );
                openIdConfig = await configurationManager.GetConfigurationAsync(CancellationToken.None);
            }

            // If user defined x-issuer then use this value instead of discovered value
            openIdConfig.Issuer = ianvsContext.SecurityScheme.Issuer ?? openIdConfig.Issuer;

            return openIdConfig;
        }

        public bool CanAuthenticate(HttpContext httpContext, IanvsContext ianvsContext)
        {
            // Get token value, only header and cookie are supported
            string tokenValue = GetTokenValue(httpContext, ianvsContext);
            if (!string.IsNullOrWhiteSpace(tokenValue))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retireves the token value from the incoming request
        /// </summary>
        /// <param name="httpContext">The incoming request HTTP context</param>
        /// <param name="ianvsContext">The incoming request Ianvs context</param>
        /// <returns>The token value</returns>
        private string GetTokenValue(HttpContext httpContext, IanvsContext ianvsContext)
        {
            string tokenIn = ianvsContext.SecurityScheme.In?? "header";
            return tokenIn switch
            {
                "header" => GetTokenFromHeader(httpContext),
                "cookie" => GetTokenFromCookie(httpContext, ""),
                _ => GetTokenFromHeader(httpContext)
            };
        }

        /// <summary>
        /// Gets the value of the JWT token from the request cookies
        /// </summary>
        /// <param name="httpContext">The incoming reuqest HTTP context</param>
        /// <param name="name">The name of the cookie containing the JWT value</param>
        /// <returns>The encoded token value</returns>
        private string GetTokenFromCookie(HttpContext httpContext, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the JWT token from the request Authorization header
        /// </summary>
        /// <param name="httpContext">The incoming reuqest HTTP context</param>
        /// <returns>The encoded token value</returns>
        private string GetTokenFromHeader(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                return token.ToString().Replace("Bearer ", "");
            }
            else
            {
                return null;
            }
        }
    }
}