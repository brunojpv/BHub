using BHub.Api.CrossCutting.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Collections.Generic;
using Constants = BHub.Infra.Environments.Constants;

namespace BHub.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
            {
                HostName = Constants.HOST_RABBITMQ,
                Port = int.Parse(Constants.PORT_RABBITMQ),
                UserName = Constants.USERNAME_RABBITMQ,
                Password = Constants.PASSWORD_RABBITMQ
            });

            services.AddCors(options =>
                options.AddPolicy(
                    "AllowAll", p =>
                    {
                        p.AllowAnyMethod();
                        p.AllowAnyHeader();
                    }));

            services.RegisterConnectionServices();
            services.RegisterDependencies();
            services.RegisterHealthChecks();

            services.AddControllers();
            services.AddSwaggerConfig("BHubApi", "Api para incluir um novo cliente a partir da fila no rabbitmq.");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(swaggerOptions =>
                {
                    swaggerOptions.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = Constants.APP_URL } };
                    });
                });
                app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "BHub.Api v1"));
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks();
            });
        }
    }
}
