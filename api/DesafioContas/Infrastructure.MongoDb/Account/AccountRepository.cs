using Desafio.Contas.Infrastructure.MongoDb.Repository;

namespace Desafio.Contas.Infrastructure.MongoDb.Account
{
    public class AccountRepository : MongoDBRepository<AccountDocument>, IAccountRepository
    {
        public AccountRepository(MongoDBSettings settings) : base(settings)
        {
        }
    }
}
