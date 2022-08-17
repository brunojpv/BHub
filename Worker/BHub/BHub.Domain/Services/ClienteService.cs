using BHub.Domain.Dtos;
using BHub.Domain.Interfaces.Repositories;
using BHub.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BHub.Domain.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ILogger<ClienteService> logger;
        private readonly IClienteRepository clienteRepository;

        public ClienteService(ILogger<ClienteService> logger, IClienteRepository clienteRepository)
        {
            this.logger = logger;
            this.clienteRepository = clienteRepository;
        }

        public async Task<bool> ExecuteCreationCliente(TemplateRabbitClienteDto templateRabbitClienteDto)
        {
            try
            {
                logger.LogInformation($"Iniciando a inclusao do cliente: {templateRabbitClienteDto.Id}.");

                var produtoFilial = await clienteRepository.SearchClienteById(templateRabbitClienteDto.Id);

                if (produtoFilial.Id == 0)
                {
                    await clienteRepository.CreateCliente(templateRabbitClienteDto.Id, templateRabbitClienteDto.RazaoSocial, templateRabbitClienteDto.Telefone, templateRabbitClienteDto.Endereco, templateRabbitClienteDto.Faturamento);

                    return true;
                }

                logger.LogError($"Cliente: {templateRabbitClienteDto.RazaoSocial} não possui registros válidos.");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro no processo de inclusao do cliente: {ex.Message}");
                return false;
            }
        }
    }
}
