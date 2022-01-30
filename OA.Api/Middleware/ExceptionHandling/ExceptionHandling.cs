using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OA.Base.Helpers;
using OA.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OA.Api.Middleware.ExceptionHandling
{
    public static class ExceptionHandling
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFature is not null)
                    {
                        await context.Response.WriteAsync
                        (
                            JsonHelper.Serialize(new ExceptionHandlingModel
                            {
                                Status = context.Response.StatusCode,
                                Message = "Internal Server Error"
                            })
                        );
                    }
                });
            });
        }

        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddFile("Logs/Home_Care_ServicesLogFile-{Date}.txt");
            loggerFactory.CreateLogger("Logs/Home_Care_ServicesLogFile-{Date}.txt");
            app.UseMiddleware<CustomExceptionHandling>();
        }
    }
}
