using BHub.Domain.Interfaces.Repositories;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BHub.Test.Services
{
    public class ClienteServiceTest
    {
        [Fact]
        public Task Test_CreateCliente()
        {
            var clienteRepository = new Mock<IClienteRepository>();

            clienteRepository.Setup(x => x.CreateCliente(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns(Task.FromResult(true));

            return Task.CompletedTask;
        }
    }
}
