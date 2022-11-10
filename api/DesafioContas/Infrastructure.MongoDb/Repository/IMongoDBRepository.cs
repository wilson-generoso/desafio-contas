using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Desafio.Contas.Infrastructure.MongoDb.Repository
{
    public interface IMongoDBRepository<TDocument> where TDocument : BaseDocument, new()
    {
        IMongoQueryable<TDocument> AsQueryable();
        Task<TDocument> Get(Guid id);
        Task<bool> Exists(Guid id);
        Task<IEnumerable<TDocument>> GetAll();
        Task Delete(Guid id);
        Task<IEnumerable<TDocument>> FindBy(Expression<Func<TDocument, bool>> filterExpression);
        Task InsertOne(TDocument document);
        Task ReplaceOne(TDocument document);
    }
}
