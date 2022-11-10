using AutoMapper;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace Desafio.Contas.Infrastructure.MongoDb.Entry
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository repository;
        private readonly IMapper mapper;

        public EntryService(IEntryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Create(Domain.Entry entity)
        {
            var document = mapper.Map<EntryDocument>(entity);
            return this.repository.InsertOne(document);
        }

        public Task Delete(Guid id) => this.repository.Delete(id);

        public async Task<Domain.Entry> Get(Guid id)
        {
            var document = await this.repository.Get(id);
            return mapper.Map<Domain.Entry>(document);
        }

        public async Task<IEnumerable<Domain.Entry>> GetEntries(Guid accountId, DateTime initialDate, DateTime? endDate = null)
        {
            var filter = this.repository.AsQueryable().Where(doc => doc.AccountId == accountId && doc.Date >= initialDate);

            if (endDate.HasValue)
                filter = filter.Where(doc => doc.Date <= endDate.Value);

            var documents = await filter.ToListAsync();

            return documents.Select(doc => mapper.Map<Domain.Entry>(doc)).ToArray();
        }

        public Task Update(Domain.Entry entity)
        {
            var document = mapper.Map<EntryDocument>(entity);
            return this.repository.ReplaceOne(document);
        }
    }
}
