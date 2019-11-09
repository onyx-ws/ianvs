using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Common
{

    /// <summary>
    /// Service endpoint represented by a Path item object in OpenApi 
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// The target endpoint relative path
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The virtual endpoint relative path. If not defined, endpoint Url is used as virtual Url
        /// </summary>
        public string IanvsUrl { get; set; }

        /// <summary>
        /// An optional, string summary, intended to apply to all operations in this endpoint
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// An optional, string description, intended to apply to all operations in this endpoint. CommonMark syntax MAY be used for rich text representation
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A definition of all operations on this endpoint
        /// </summary>
        public List<Operation> Operations { get; set; }

        /// <summary>
        /// An alternative server array to service all operations in this endpoint
        /// </summary>
        public List<Server> Servers { get; set; }
    }
}