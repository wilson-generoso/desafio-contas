using Desafio.Contas.Domain;

namespace Desafio.Contas.Infrastructure
{
    public interface IEntryService : IService<Entry>
    {
        Task<IEnumerable<Entry>> GetEntries(Guid accountId, DateTime initialDate, DateTime? endDate = null);
    }
}
