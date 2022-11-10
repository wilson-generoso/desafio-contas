using AutoMapper;

namespace Desafio.Contas.Infrastructure.MongoDb.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository repository;
        private readonly IMapper mapper;

        public AccountService(IAccountRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Create(Domain.Account entity)
        {
            var document = mapper.Map<AccountDocument>(entity);
            return this.repository.InsertOne(document);
        }

        public Task Delete(Guid id) => this.repository.Delete(id);
        
        public Task<bool> Exists(Guid accountId) => this.repository.Exists(accountId);

        public async Task<Domain.Account> Get(Guid id)
        {
            var document = await this.repository.Get(id);
            return mapper.Map<Domain.Account>(document);
        }

        public async Task<IEnumerable<Domain.Account>> GetAll()
        {
            var documents = await this.repository.GetAll();
            return documents.Select(doc => mapper.Map<Domain.Account>(doc));
        }

        public async Task<bool> HasAccountName(string accountName)
        {
            var documents = await this.repository.FindBy(doc => doc.Name == accountName);
            return documents.Any();
        }

        public Task Update(Domain.Account entity)
        {
            var document = mapper.Map<AccountDocument>(entity);
            return this.repository.ReplaceOne(document);
        }
    }
}
