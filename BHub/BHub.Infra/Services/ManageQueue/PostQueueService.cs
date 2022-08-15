using BHub.Domain.Dtos;
using BHub.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using Constants = BHub.Api.Environments.Constants;

namespace BHub.Infra.Services.ManageQueue
{
    public class PostQueueService : IPostQueueService
    {
        public readonly ILogger<PostQueueService> logger;

        public PostQueueService(ILogger<PostQueueService> logger)
        {
            this.logger = logger;
        }

        public Task<bool> ExecutePostQueue(TemplateRabbitClienteDto templateRabbitClienteDto)
        {
            logger.LogInformation($"Incluindo informações do cliente {templateRabbitClienteDto.RazaoSocial} na fila do Rabbit.");

            var factory = new ConnectionFactory
            {
                HostName = Constants.HOST_RABBITMQ,
                Port = int.Parse(Constants.PORT_RABBITMQ),
                UserName = Constants.USERNAME_RABBITMQ,
                Password = Constants.PASSWORD_RABBITMQ
            };

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    var message = JsonConvert.SerializeObject(templateRabbitClienteDto);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(Constants.NAME_EXCHANGE_RABBITMQ,
                        "",
                        properties,
                        body);
                }

                logger.LogInformation("Inclusão feita com sucesso.");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Erro na inclusão na fila: {ex.Message}");
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
