﻿using Microsoft.AspNetCore.Http;
using Onyx.Ianvs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs.Transformation
{
    /// <summary>
    /// Responsible for performing request/response transformations
    /// </summary>
    public class TransformationMiddleware
    {
        private readonly RequestDelegate _next;

        public TransformationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IanvsContext ianvsContext)
        {
            // TODO: Implement Transformation
            // https://github.com/onyx-ws/ianvs/issues/10

            ianvsContext.BackendMessage = new BackendMessage();

            await _next(httpContext);

            ianvsContext.Response = ianvsContext.BackendResponse.Content;
        }
    }
}