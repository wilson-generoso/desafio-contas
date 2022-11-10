using Desafio.Contas.Domain;
using Desafio.Contas.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Contas.Application.Test.Mocks
{
    internal class UpdateAccountServiceMock : IAccountService
    {
        private Account account;

        public Task Create(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Account> Get(Guid id)
        {
            if(account == null)
                account = new Account { Id = id, Name = "AccountService", Balance = 100m, CreatedAt = DateTime.Now };

            return Task.FromResult(account);
        }

        public Task<IEnumerable<Account>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasAccountName(string accountName)
        {
            throw new NotImplementedException();
        }

        public virtual Task Update(Account entity)
        {
            account = entity;
            return Task.CompletedTask;
        }
    }

    internal class UpdateFailAccountServiceMock : UpdateAccountServiceMock
    {
        public override Task Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
