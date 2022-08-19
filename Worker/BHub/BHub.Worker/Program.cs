using BHub.Domain.Interfaces.Repositories;
using BHub.Domain.Interfaces.Services;
using BHub.Domain.Repositories;
using BHub.Domain.Services;
using BHub.Domain.Services.Interfaces;
using BHub.Domain.Services.ManageQueue;
using BHub.Infra.Data.Connection.Factories;
using BHub.Infra.Data.Connection.Factories.Interfaces;
using BHub.Infra.Extension;
using BHub.Infra.Extension.Interfaces;
using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Serilog;
using ConnectionFactory = RabbitMQ.Client.ConnectionFactory;
using Constants = BHub.Infra.Environments.Constants;

namespace BHub.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).UseSerilog().Build().Run();
        }

        public IConfiguration Configuration { get; }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
                    {
                        HostName = Constants.HOST_RABBITMQ,
                        Port = int.Parse(Constants.PORT_RABBITMQ),
                        UserName = Constants.USERNAME_RABBITMQ,
                        Password = Constants.PASSWORD_RABBITMQ
                    });

                    SqlConnectionFactory factory = new(Constants.APPLICATION_NAME, Constants.HOST, Constants.DATABASE, Constants.USER, Constants.PASS, 0);

                    services.AddSingleton<IDbConnectionFactory>(c => factory);

                    services.AddSingleton<IConsumerQueueService, ConsumerQueueService>();
                    services.AddSingleton<IClienteRepository, ClienteRepository>();
                    services.AddSingleton<IClienteService, ClienteService>();
                    services.AddSingleton<IMailExtension, MailExtension>();

                    services.AddHostedService<Worker>();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                app.UseAllElasticApm(Configuration);
            }
        }
    }
}
