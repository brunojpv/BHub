using BHub.Domain.Dtos;
using BHub.Infra.Services.Interfaces;
using System.Threading.Tasks;

namespace BHub.Test.Services
{
    public class PostQueueServiceTest : IPostQueueService
    {
        public Task<bool> ExecutePostQueue(TemplateRabbitClienteDto templateRabbitClienteDto)
        {
            return Task.FromResult(true);
        }
    }
}
