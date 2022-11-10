using Desafio.Contas.Infrastructure.MongoDb.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace Desafio.Contas.Infrastructure.MongoDb.Entry
{
    [BsonCollection("Entries")]
    public class EntryDocument : BaseDocument
    {
        [BsonRequired]
        public Guid AccountId { get; set; }

        [BsonRequired]
        public string Description { get; set; }

        [BsonRequired]
        public DateTime Date { get; set; }
        
        [BsonRequired] 
        public DateTime CreatedAt { get; set; }

        [BsonRequired]
        public decimal PreviousBalance { get; set; }

        [BsonRequired]
        public decimal Value { get; set; }

        [BsonRequired]
        public string Type { get; set; }
    }
}
