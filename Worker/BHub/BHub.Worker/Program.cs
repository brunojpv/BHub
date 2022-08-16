using BHub.Infra.Data.Connection.Factories;
using BHub.Infra.Data.Connection.Factories.Interfaces;
using BHub.Infra.Services.Interfaces;
using BHub.Infra.Services.ManageQueue;
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
                    SqlConnectionFactory factory = new(Constants.APPLICATION_NAME, Constants.HOST, Constants.DATABASE,
                     Constants.USER, Constants.PASS, 0);
                    services.AddSingleton<IDbConnectionFactory>(c => factory);

                    services.AddSingleton<IConsumerQueueService, ConsumerQueueService>();

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
