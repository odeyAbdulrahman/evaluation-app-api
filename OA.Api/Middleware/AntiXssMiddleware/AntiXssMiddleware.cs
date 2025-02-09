﻿namespace evaluation_app.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using OA.Dtos;

    namespace AntiXssMiddleware.Middleware
    {
        public class AntiXssMiddleware
        {
            private readonly RequestDelegate _next;
            private ExceptionHandlingModel _error;
            private readonly int _statusCode = (int)HttpStatusCode.BadRequest;

            public AntiXssMiddleware(RequestDelegate next)
            {
                _next = next ?? throw new ArgumentNullException(nameof(next));
            }

            public async Task Invoke(HttpContext context)
            {
                // Check XSS in URL
                if (!string.IsNullOrWhiteSpace(context.Request.Path.Value))
                {
                    var url = context.Request.Path.Value;

                    if (CrossSiteScriptingValidation.IsDangerousString(url, out _))
                    {
                        await RespondWithAnError(context).ConfigureAwait(false);
                        return;
                    }
                }

                // Check XSS in query string
                if (!string.IsNullOrWhiteSpace(context.Request.QueryString.Value))
                {
                    var queryString = WebUtility.UrlDecode(context.Request.QueryString.Value);

                    if (CrossSiteScriptingValidation.IsDangerousString(queryString, out _))
                    {
                        await RespondWithAnError(context).ConfigureAwait(false);
                        return;
                    }
                }

                // Check XSS in request content
                var originalBody = context.Request.Body;
                try
                {
                    var content = await ReadRequestBody(context);

                    if (CrossSiteScriptingValidation.IsDangerousString(content, out _))
                    {
                        await RespondWithAnError(context).ConfigureAwait(false);
                        return;
                    }
                    await _next(context).ConfigureAwait(false);
                }
                finally
                {
                    context.Request.Body = originalBody;
                }
            }

            private static async Task<string> ReadRequestBody(HttpContext context)
            {
                var buffer = new MemoryStream();
                await context.Request.Body.CopyToAsync(buffer);
                context.Request.Body = buffer;
                buffer.Position = 0;
                var encoding = Encoding.UTF8;
                var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
                context.Request.Body.Position = 0;

                return requestContent;
            }

            private async Task RespondWithAnError(HttpContext context)
            {
                context.Response.Clear();
                context.Response.Headers.AddHeaders();
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = _statusCode;

                if (_error == null)
                {
                    _error = new ExceptionHandlingModel
                    {
                        Message = "Error from Anti Xss Middleware",
                        Status = 500
                    };
                }

                await context.Response.WriteAsync(_error.ToJSON());
            }
        }

        public static class AntiXssMiddlewareExtension
        {
            public static IApplicationBuilder UseAntiXssMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<AntiXssMiddleware>();
            }
        }
    }
}
