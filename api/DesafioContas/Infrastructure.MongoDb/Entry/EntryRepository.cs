using Desafio.Contas.Infrastructure.MongoDb.Repository;

namespace Desafio.Contas.Infrastructure.MongoDb.Entry
{
    public class EntryRepository : MongoDBRepository<EntryDocument>, IEntryRepository
    {
        public EntryRepository(MongoDBSettings settings) : base(settings)
        {
        }
    }
}
