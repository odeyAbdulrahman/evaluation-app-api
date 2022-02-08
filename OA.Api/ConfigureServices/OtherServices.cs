using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using System;
using OA.Api.BackgroundJob;
using OA.Api.ConfigureServices;
using OA.Api.Common.FeatureFlagsBase;
using OA.Api.UnitOfWork;
using OA.Repo;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace OA.api.ConfigureServices
{
    public class OtherServices : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            //Add Background Job
            services.AddSingleton<IBackgroundJob, BackgroundJob>();
            //Add Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //add Unit Of Work Infrastructure
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //Add Feature Management
            services.AddFeatureManagement().UseDisabledFeaturesHandler(new RedirectDisabledFeatureHandler());
            //Add Swagger Gen 1
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Evaluation.App",
                    Version = "1.0",
                    Description = "beta version",
                    TermsOfService = new Uri("https://google.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Email = "",
                    },
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
            services.AddControllers().AddNewtonsoftJson();
            //Add Auto mapper 
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "AllowOrigin",
                    builder => {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    });
            });
        }
    }
}
