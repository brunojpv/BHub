using BHub.Domain.Dtos;
using System.Threading.Tasks;

namespace BHub.Domain.Interfaces
{
    public interface IPostQueueService
    {
        Task<bool> ExecutePostQueue(TemplateRabbitClienteDto templateRabbitClienteDto);
    }
}
