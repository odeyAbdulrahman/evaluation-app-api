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
    public class CustomExceptionHandling
    {
        private readonly RequestDelegate Next;
        private readonly ILogger Logger;

        public CustomExceptionHandling(RequestDelegate next, ILogger<CustomExceptionHandling> logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await Next(httpContext);
            }
            catch (TimeoutException ex)
            {
                Logger.LogError($"*************** Controller name: ${httpContext.Request.Path.Value} / Action Type : ${httpContext.Request.Method} ***************");
                Logger.LogError(ex, "------ Timeout Exception ------");
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.LogError($"*************** Controller name: ${httpContext.Request.Path.Value} / Action Type : ${httpContext.Request.Method} ***************");
                Logger.LogError(ex, "------ Unauthorized Access ------");
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            catch (DivideByZeroException ex)
            {
                Logger.LogError($"*************** Controller name: {httpContext.Request.Path.Value} / Action Type : {httpContext.Request.Method} ***************");
                Logger.LogError(ex, "------ Divide By Zero ------");
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError($"*************** Controller name: {httpContext.Request.Path.Value} / Action Type : {httpContext.Request.Method} ***************");
                Logger.LogError(ex, "------ Something went wrong ------");
                await HandleExceptionAsync(httpContext, ex.Message);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, string ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync
            (
             JsonHelper.Serialize(
                new ExceptionHandlingModel
                {
                    Status = context.Response.StatusCode,
                    Message = $"Internal Server Error From Custom Middleware ({ex})"
                })
            );
        }
    }
}
