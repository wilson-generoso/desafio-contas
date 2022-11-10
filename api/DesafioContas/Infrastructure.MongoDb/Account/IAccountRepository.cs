using Desafio.Contas.Infrastructure.MongoDb.Repository;

namespace Desafio.Contas.Infrastructure.MongoDb.Account
{
    public interface IAccountRepository : IMongoDBRepository<AccountDocument>
    {
    }
}
