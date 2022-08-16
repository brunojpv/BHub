using System.Threading.Tasks;

namespace BHub.Infra.Services.Interfaces
{
    public interface IConsumerQueueService
    {
        Task ExecuteConsumer();
    }
}
