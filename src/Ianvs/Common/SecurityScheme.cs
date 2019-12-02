using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Common
{
    /// <summary>
    /// Represents an Open API Security Scheme object
    /// </summary>
    public class SecurityScheme
    {
        /// <summary>
        /// The security scheme name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The security scheme type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Where the authentication type is sent in the request; supported values: header, cookie
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// The security scheme; bearer, oauth, key, etc.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Used as a hint for the value name: bearer, cookie name
        /// </summary>
        public string BearerFormat { get; set; }

        /// <summary>
        /// The Url of the auth authority openId configuration discovery Url
        /// </summary>
        public string OpenIdConnectUrl { get; set; }

        /// <summary>
        /// The Url of the auth credentials issuer/authority
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The list of valid audiences for the security scheme
        /// </summary>
        public string[] Audiences { get; set; }
    }
}