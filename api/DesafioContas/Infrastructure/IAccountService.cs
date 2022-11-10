using Desafio.Contas.Domain;

namespace Desafio.Contas.Infrastructure
{
    public interface IAccountService : IService<Account>
    {
        Task<IEnumerable<Account>> GetAll();
        Task<bool> Exists(Guid accountId);
        Task<bool> HasAccountName(string accountName);
    }
}
