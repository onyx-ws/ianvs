using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Http
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Creates a 404 Not Found HTTP response
        /// https://tools.ietf.org/html/rfc7231#section-6.5.4
        /// </summary>
        /// <param name="httpResponse">The Microsoft.AspNetCore.Http.HttpResponse</param>
        /// <returns>A task that represents the completion of the write operation</returns>
        public static Task WriteNotFoundResponseAsync(this HttpResponse httpResponse)
        {
            httpResponse.StatusCode = 404;
            return httpResponse.WriteAsync("");
        }

        /// <summary>
        /// Creates a 405 Method Not Allowed HTTP response
        /// https://tools.ietf.org/html/rfc7231#section-6.5.5
        /// </summary>
        /// <param name="httpResponse">The Microsoft.AspNetCore.Http.HttpResponse</param>
        /// <returns>A task that represents the completion of the write operation</returns>
        public static Task WriteMethodNotAllowedResponseAsync(this HttpResponse httpResponse)
        {
            httpResponse.StatusCode = 405;
            return httpResponse.WriteAsync("");
        }
    }
}