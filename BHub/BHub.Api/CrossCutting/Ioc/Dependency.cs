using BHub.Infra.Data.Connection.Factories;
using BHub.Infra.Data.Connection.Factories.Interfaces;
using BHub.Infra.Environments;
using BHub.Infra.Services.Interfaces;
using BHub.Infra.Services.ManageQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;

namespace BHub.Api.CrossCutting.Ioc
{
    public static class Dependency
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.RegisterServices();
            services.RegisterRepositories();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPostQueueService, PostQueueService>();

            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection repositories)
        {
            return repositories;
        }

        public static IServiceCollection RegisterConnectionServices(this IServiceCollection services)
        {
            SqlConnectionFactory factory = new(Constants.APPLICATION_NAME, Constants.HOST, Constants.DATABASE, Constants.USER, Constants.PASS, 0);
            services.AddSingleton<IDbConnectionFactory>(c => factory);

            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services,
            string title,
            string description)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = title,
                        Version = "v1",
                        Description = description,
                        Contact = new OpenApiContact
                        {
                            Name = "Bruno Vieira",
                            Email = "brunojpv@gmail.com"
                        },
                        License = new OpenApiLicense { Name = "Empreendimentos Zenotech LTDA,All Rights Reserved." }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection RegisterHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddSqlServer(Constants.ConnectionString);

            return services;
        }

        public static IEndpointRouteBuilder MapHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/api/health",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                statusApplication = report.Status.ToString(),
                                healthChecks = report.Entries.Select(e => new
                                {
                                    check = e.Key,
                                    ErrorMessage = e.Value.Exception?.Message,
                                    status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

            return endpoints;
        }
    }
}
