using MongoDB.Bson.Serialization.Attributes;

namespace Desafio.Contas.Infrastructure.MongoDb.Repository
{
    public abstract class BaseDocument
    {
        [BsonId]
        
        public virtual Guid Id { get; set; }
    }
}
