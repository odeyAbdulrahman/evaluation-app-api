using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OA.Base.Enums;
using OA.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OA.Api.Filters
{
    public class ConsumerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("Consumer"))
            {
                var Consumer = context.HttpContext.Request.Headers["Consumer"];
                if (Consumer == "")
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                else
                {
                    if (long.TryParse(Consumer, out _))
                    {
                        if (!Enum.IsDefined(typeof(EnumConsumer), long.Parse(Consumer)))
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
