using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Configuration
{
    /// <summary>
    /// An object representing a Server hosting services
    /// </summary>
    public class Server
    {
        /// <summary>
        /// A URL to the target host. This URL supports Server Variables. Variable substitutions will be made when a variable is named in {brackets}
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// An optional string describing the host designated by the URL. CommonMark syntax MAY be used for rich text representation
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A map between a variable name and its value. The value is used for substitution in the server's URL template
        /// </summary>
        public List<Variable> Variables { get; set; }
    }
}