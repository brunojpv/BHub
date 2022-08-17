using BHub.Domain.Interfaces.Repositories;
using BHub.Domain.Models;
using BHub.Infra.Data.Connection.Factories.Interfaces;
using Dapper;
using System;
using System.Threading.Tasks;

namespace BHub.Domain.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IDbConnectionFactory connectionFactory;

        public ClienteRepository(IDbConnectionFactory _connectionFactory)
        {
            connectionFactory = _connectionFactory;
        }

        public async Task<Cliente> SearchClienteById(int id)
        {
            try
            {
                using var connection = connectionFactory.CreateConnectionOpened();
                var produtoFilial = await connection.QueryFirstAsync<Cliente>(@"Select * From Cliente Where Id = @Id", new { id }, commandTimeout: 0);
                connection.Close();

                return produtoFilial;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task CreateCliente(int id, string razaoSocial, string telefone, string endereco, decimal faturamento)
        {
            try
            {
                using var connection = connectionFactory.CreateConnectionOpened();
                await connection.ExecuteAsync(@"Insert Into Clientes Values(@Id, @RazaoSocial, @Telefone, @Endereco, GetDate(), @Faturamento)", new { id, razaoSocial, telefone, endereco, faturamento }, commandTimeout: 0);
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
