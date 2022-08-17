using System.Threading.Tasks;

namespace BHub.Domain.Services.Interfaces
{
    public interface IConsumerQueueService
    {
        Task ExecuteConsumer();
    }
}
