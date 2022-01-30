using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace OA.Api.Filters
{
    public class ApiKeyAuthAttribute
    {
        //private const string ApiKeyHeaderName = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            //var Configration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            //var apikey = Configration.GetValue<string>(key: "ApiKey");
            //if (!apikey.Equals(potentialApiKey))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}
            await next();
        }
    }
}
