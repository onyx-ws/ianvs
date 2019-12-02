using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Common
{
    /// <summary>
    /// Represents a Security Requirements Open Api object
    /// </summary>
    public class SecurityRequirement
    {
        /// <summary>
        /// The required security scheme name
        /// </summary>
        public string SchemeName { get; set;}

        /// <summary>
        /// The authorizations scopes that must be granted to the caller
        /// </summary>
        public string[] Scopes { get; set; }
    }
}
