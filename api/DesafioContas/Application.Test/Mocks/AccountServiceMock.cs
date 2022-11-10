using Desafio.Contas.Domain;
using Desafio.Contas.Infrastructure;
using Moq;
using System.Collections.Concurrent;

namespace Desafio.Contas.Application.Test.Mocks
{
    public static class AccountServiceMock
    {
        public static Guid UpdateAccoundId = new Guid("10D4EB6B-A8A8-40D9-9B18-A8025796B4D6");

        public static Mock<IAccountService> BuildAddAccountServiceMock()
        {
            var mock = new Mock<IAccountService>();

            mock.Setup(x => x.Create(It.IsAny<Account>())).Returns((Account account) => Task.CompletedTask);

            return mock;
        }

        public static Mock<IAccountService> BuildValidateAddAccountMock()
        {
            var mock = new Mock<IAccountService>();

            mock.Setup(x => x.HasAccountName(It.IsAny<string>())).Returns((string name) => Task.FromResult(name == "ValidateAddAccount"));

            return mock;
        }

        public static Mock<IAccountService> BuildValidateAccountMock()
        {
            var mock = new Mock<IAccountService>();

            mock.Setup(x => x.Exists(It.IsAny<Guid>())).Returns((Guid id) => Task.FromResult(id.Equals(UpdateAccoundId)));

            return mock;
        }

        internal static Mock<IAccountService> BuildAccountServiceMock()
        {
            var mock = new Mock<IAccountService>();

            mock.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Guid id) => Task.FromResult(new Account { Id = id, Name = "AccountService", Balance = 100m }));

            return mock;
        }

        internal static Mock<IAccountService> BuildGetAccountsServiceMock()
        {
            var mock = new Mock<IAccountService>();

            mock.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Guid id) => Task.FromResult(id == UpdateAccoundId ? new Account { Id = id, Name = "AccountService", Balance = 100m } : null));

            mock.Setup(x => x.GetAll()).Returns(() => Task.FromResult(new Account[]
            {
                new Account { Id = UpdateAccoundId, Name = "AccountService", Balance = 100m },
                new Account { Id = Guid.NewGuid(), Name = "AccountService1", Balance = 70m },
                new Account { Id = Guid.NewGuid(), Name = "AccountService2", Balance = 50m },
                new Account { Id = Guid.NewGuid(), Name = "AccountService3", Balance = 30m }
            }.AsEnumerable()));

            return mock;
        }

        internal static Mock<IAccountService> BuildSearchEntriesAccountMock()
        {
            var mock = new Mock<IAccountService>();

            mock.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Guid id) => Task.FromResult(id == UpdateAccoundId ? new Account
            {
                Id = AccountServiceMock.UpdateAccoundId,
                Balance = 10,
                Name = "SearchTest",
                CreatedAt = DateTime.Now.AddDays(-1)
            } : null));

            return mock;
        }
    }
}
