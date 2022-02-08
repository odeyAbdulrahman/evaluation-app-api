using evaluation_app.Middleware.AntiXssMiddleware.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OA.api.ConfigureServices;
using OA.Api.Common.AppSettingsBase;
using OA.Api.ConfigureServices;
using OA.Api.Middleware.ExceptionHandling;
using OA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evaluation_app
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }
        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServiceInAssembly(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory, AppDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evaluation app v1"));
            }
            else
            {
                app.UseHsts();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evaluation app v1"));
            }
            app.UseAntiXssMiddleware();
            app.ConfigureCustomExceptionHandler(loggerFactory);
            app.UseHttpsRedirection();
            app.UseCors("AllowOrigin");
            app.UseRouting().UseCors(options => options.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((Host) => true).AllowCredentials());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndPoints();
            serviceProvider.AppSetting(db).Wait();
        }
    }
}
