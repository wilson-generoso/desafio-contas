using Desafio.Contas.Infrastructure.MongoDb.Repository;

namespace Desafio.Contas.Infrastructure.MongoDb.Entry
{
    public interface IEntryRepository : IMongoDBRepository<EntryDocument>
    {
    }
}
