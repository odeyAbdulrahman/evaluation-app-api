using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OA.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OA.Api.ConfigureServices
{
    public static class Endpoints
    {
        internal static void UseEndPoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/Health", new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        context.Response.ContentType = "application/json";
                        var responce = new HealthCheckResponseModel
                        {
                            Status = nameof(report.Status),
                            Checks = report.Entries.Select(x => new HealthCheckModel
                            {
                                Component = x.Key,
                                Status = nameof(x.Value.Status),
                                Description = x.Value.Description
                            }),
                            Duration = report.TotalDuration
                        };
                        await context.Response.WriteAsync(text: JsonConvert.SerializeObject(responce));
                    },
                }).RequireAuthorization();
            });
        }
    }
}
