using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Desafio.Contas.Infrastructure.MongoDb.Repository
{
    public class MongoDBRepository<TDocument> : IMongoDBRepository<TDocument>
        where TDocument : BaseDocument, new()
    {
        protected readonly IMongoDatabase _database;
        protected readonly IMongoCollection<TDocument> _collection;

        public MongoDBRepository(MongoDBSettings settings)
        {
            var mongoSettings = new MongoClientSettings
            {
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                Server = new MongoServerAddress(settings.Host, settings.Port),
                //Codigo caso possua autenticação configurado para o mongodb
                //Credential = new MongoCredential(settings.AuthenticationMechanism, new MongoInternalIdentity(settings.DatabaseName, settings.Username), new PasswordEvidence(settings.Password))
            };

            _database = new MongoClient(mongoSettings).GetDatabase(settings.DatabaseName);
            _collection = this.GetCollection<TDocument>();
        }

        protected IMongoCollection<TDoc> GetCollection<TDoc>()
        {
            var collectionName = typeof(TDoc).GetCustomAttribute<BsonCollectionAttribute>(true)?.CollectionName ?? typeof(TDoc).Name;
            return _database.GetCollection<TDoc>(collectionName);
        }

        public virtual IMongoQueryable<TDocument> AsQueryable() => _collection.AsQueryable();

        public virtual async Task<IEnumerable<TDocument>> FindBy(Expression<Func<TDocument, bool>> filterExpression) => (await _collection.FindAsync(filterExpression)).ToEnumerable();

        private FilterDefinition<TDocument> GetFilterById(Guid id) => Builders<TDocument>.Filter.Eq(doc => doc.Id, id);

        public async Task Delete(Guid id)
        {
            var filter = GetFilterById(id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task InsertOne(TDocument document) => await _collection.InsertOneAsync(document);

        public async Task ReplaceOne(TDocument document)
        {
            var filter = GetFilterById(document.Id);
            await _collection.ReplaceOneAsync(filter, document);
        }

        public async Task<TDocument> Get(Guid id)
        {
            var filter = GetFilterById(id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TDocument>> GetAll() => await _collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync();

        public async Task<bool> Exists(Guid id)
        {
            var filter = GetFilterById(id);
            return await _collection.Find(filter).AnyAsync();
        }
    }
}
