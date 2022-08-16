using BHub.Domain.Dtos;
using BHub.Infra.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Constants = BHub.Infra.Environments.Constants;

namespace BHub.Infra.Services.ManageQueue
{
    public class ConsumerQueueService : IConsumerQueueService
    {
        private ILogger<ConsumerQueueService> logger;
        //private readonly ICreateBulletinService createBulletinService;
        private readonly IConnectionFactory connectionFactory;

        public ConsumerQueueService(ILogger<ConsumerQueueService> _logger,
            //ICreateBulletinService _createBulletinService,
            IConnectionFactory _connectionFactory)
        {
            logger = _logger;
            //createBulletinService = _createBulletinService;
            connectionFactory = _connectionFactory;
        }

        public async Task ExecuteConsumer()
        {
            logger.LogInformation("Iniciando consumo da fila de Geração do Boletim.");

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);

                channel.BasicConsume(queue: Constants.NAME_QUEUE_RABBITMQ,
                    autoAck: false,
                    consumer: consumer);

                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var obj = JsonConvert.DeserializeObject<TemplateRabbitClienteDto>(message);
                        Console.WriteLine(" [x] Recebido {0}", message);

                        var resp = true;// await createBulletinService.ExecuteCreationBolletin(obj);

                        if (resp)
                        {
                            channel.BasicAck(ea.DeliveryTag, false);
                            logger.LogInformation($"Foi feita com sucesso a migração do Cliente {obj.RazaoSocial}!");
                        }
                        else
                        {
                            logger.LogError($"Erro na migração do Cliente {obj.RazaoSocial}! Item de volta á fila.");
                            channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Erro na criação do Boletim! Item de volta á fila. Error: {ex.Message}");
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };
                Thread.Sleep(Timeout.Infinite);
            }
            logger.LogInformation(
                "Finalizando consumo da fila de Geração do Boletim. Fila aguardando novas inserções...");
        }
    }
}
