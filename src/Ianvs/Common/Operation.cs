using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Common
{
    public class Operation
    {
        /// <summary>
        /// The HTTP method name - GET; POST; etc.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// A short summary of what the operation does
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation behavior. CommonMark syntax MAY be used for rich text representation
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique string used to identify the operation. The id MUST be unique among all operations described in the API. Tools and libraries MAY use the operationId to uniquely identify an operation, therefore, it is RECOMMENDED to follow common programming naming conventions
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// An alternative server array to service this operation. If an alternative server object is specified at the Path Item Object or Root level, it will be overridden by this value
        /// </summary>
        public List<Server> Servers { get; set; }

        /// <summary>
        /// An alternative random balancer mode used to select the server to service this operation. If a load balancer mode is specified at the Path Item Object or Root level, it will be overridden by this value. If no mode is specified at any level, default is random
        /// </summary>
        public string LoadBalancerMethod { get; set; }

        /// <summary>
        /// A list of parameters that are applicable for this operation. If a parameter is already defined at the Path Item, the new definition will override it but can never remove it
        /// </summary>
        public List<Parameter> Parameters { get; set; }
    }
}