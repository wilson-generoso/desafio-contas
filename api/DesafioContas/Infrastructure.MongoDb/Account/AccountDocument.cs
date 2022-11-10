using Desafio.Contas.Infrastructure.MongoDb.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace Desafio.Contas.Infrastructure.MongoDb.Account
{
    [BsonCollection("Accounts")]
    public class AccountDocument : BaseDocument
    {
        [BsonRequired]
        public string Name { get; set; }

        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonRequired]
        public decimal Balance { get; set; }
    }
}
