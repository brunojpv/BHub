using BHub.Domain.Dtos;
using BHub.Domain.Interfaces.Services;
using BHub.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Constants = BHub.Infra.Environments.Constants;

namespace BHub.Domain.Services.ManageQueue
{
    public class ConsumerQueueService : IConsumerQueueService
    {
        private ILogger<ConsumerQueueService> logger;
        private readonly IClienteService clienteService;
        private readonly IConnectionFactory connectionFactory;

        public ConsumerQueueService(ILogger<ConsumerQueueService> _logger,
            IClienteService _clienteService,
            IConnectionFactory _connectionFactory)
        {
            logger = _logger;
            clienteService = _clienteService;
            connectionFactory = _connectionFactory;
        }

        public async Task ExecuteConsumer()
        {
            logger.LogInformation("Iniciando consumo da fila de inclusao do Cliente.");

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

                        var resp = await clienteService.ExecuteCreationCliente(obj);

                        if (resp)
                        {
                            channel.BasicAck(ea.DeliveryTag, false);
                            logger.LogInformation($"Foi feita com sucesso a inclusao do Cliente {obj.RazaoSocial}!");
                        }
                        else
                        {
                            logger.LogError($"Erro na inclusao do Cliente {obj.RazaoSocial}! Item de volta á fila.");
                            channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Erro na inclusao do Cliente! Item de volta á fila. Error: {ex.Message}");
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };
                Thread.Sleep(Timeout.Infinite);
            }
            logger.LogInformation("Finalizando consumo da fila de inclusao do Cliente. Fila aguardando novas inserções...");
        }
    }
}
