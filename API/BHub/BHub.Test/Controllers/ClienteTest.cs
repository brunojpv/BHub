using BHub.Domain.Dtos;
using BHub.Test.Services;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace BHub.Test.Controllers
{
    public class ClienteTest
    {
        public PostQueueServiceTest postQueueService;

        public ClienteTest()
        {
            postQueueService = new PostQueueServiceTest();
        }

        [Fact]
        public async Task CreateClienteTest()
        {
            var template = Substitute.For<TemplateRabbitClienteDto>();
            var retorno = await postQueueService.ExecutePostQueue(template);

            Assert.IsType<bool>(retorno);
        }
    }
}
