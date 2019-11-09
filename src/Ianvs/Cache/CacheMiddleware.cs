using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Cache
{
    /// <summary>
    /// Middleware to handle caching operations
    /// </summary>
    public class CacheMiddleware
    {
        private readonly RequestDelegate _next;

        public CacheMiddleware(RequestDelegate next)
        {
            // TODO: Implement caching 
            // https://github.com/onyx-ws/ianvs/issues/15
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}