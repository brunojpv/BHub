using BHub.Domain.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BHub.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumerQueueService consumerQueueService;

        public Worker(ILogger<Worker> logger, IConsumerQueueService _consumerQueueService)
        {
            _logger = logger;
            consumerQueueService = _consumerQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await consumerQueueService.ExecuteConsumer();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
