using System.Data;

namespace BHub.Infra.Data.Connection.Factories.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbConnection CreateConnectionOpened();
    }
}
