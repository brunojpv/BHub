using BHub.Domain.Dtos;
using System.Threading.Tasks;

namespace BHub.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        Task<bool> ExecuteCreationCliente(TemplateRabbitClienteDto templateRabbitClienteDto);
    }
}
