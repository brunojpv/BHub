using BHub.Domain.Dtos;
using System.Threading.Tasks;

namespace BHub.Infra.Services.Interfaces
{
    public interface IPostQueueService
    {
        Task<bool> ExecutePostQueue(TemplateRabbitClienteDto templateRabbitClienteDto);
    }
}
