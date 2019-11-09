using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Http
{
    public static class HttpRequestExtensions
    {
        public async static Task<string> ReadAsStringAsync(this HttpRequest request)
        {
            string body = "";
            request.EnableBuffering();
            using (var reader = new StreamReader(request.Body))
            {
                body = await reader.ReadToEndAsync();
                request.Body.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }

            return body;
        }
    }
}
