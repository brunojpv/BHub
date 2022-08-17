using BHub.Domain.Models;
using System.Threading.Tasks;

namespace BHub.Domain.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        Task<Cliente> SearchClienteById(int id);
        Task CreateCliente(int id, string razaoSocial, string telefone, string endereco, decimal faturamento);
    }
}
