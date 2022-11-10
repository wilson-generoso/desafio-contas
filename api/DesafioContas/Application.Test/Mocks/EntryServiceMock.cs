using Desafio.Contas.Domain;
using Desafio.Contas.Infrastructure;
using Moq;

namespace Desafio.Contas.Application.Test.Mocks
{
    internal class EntryServiceMock
    {
        public static IMock<IEntryService> BuildAddEntryServiceMock()
        {
            var mock = new Mock<IEntryService>();

            mock.Setup(x => x.Create(It.IsAny<Entry>())).Returns((Entry entry) => Task.CompletedTask);

            return mock;
        }

        public static Mock<IEntryService> BuildSearchFeirasServiceMock(IAccountService accountService, Guid accountId)
        {
            var mock = new Mock<IEntryService>();

            var account = accountService.Get(accountId).Result;

            var entradas = new Entry[]
            {
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-35), Description = "Entrada 1", PreviousBalance = 100m, Type = EntryType.Debit, Value = 30m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-35), Description = "Entrada 2", PreviousBalance = 70m, Type = EntryType.Credit, Value = 5m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-34), Description = "Entrada 3", PreviousBalance = 75m, Type = EntryType.Debit, Value = 15m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-30), Description = "Entrada 4", PreviousBalance = 60m, Type = EntryType.Debit, Value = 10m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-25), Description = "Entrada 5", PreviousBalance = 50m, Type = EntryType.Credit, Value = 20m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-9), Description = "Entrada 6", PreviousBalance = 70m, Type = EntryType.Debit, Value = 25m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-9), Description = "Entrada 7", PreviousBalance = 45m, Type = EntryType.Credit, Value = 10m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-9), Description = "Entrada 8", PreviousBalance = 55m, Type = EntryType.Debit, Value = 15m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-7), Description = "Entrada 9", PreviousBalance = 30m, Type = EntryType.Debit, Value = 10m },
                new Entry { Id = Guid.NewGuid(), Account = account, CreatedAt = DateTime.Now, Date = DateTime.Now.Date.AddDays(-7), Description = "Entrada 10", PreviousBalance = 20m, Type = EntryType.Debit, Value = 10m },
            };

            mock.Setup(x => x.GetEntries(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()))
                .Returns((Guid accountId, DateTime initialDate, DateTime? endDate) =>
                {
                    var entradasFound = entradas.Where(e => e.Account.Id == accountId && e.Date >= initialDate);

                    if (endDate.HasValue)
                        entradasFound = entradasFound.Where(x => x.Date <= endDate.Value);

                    return Task.FromResult(entradasFound);
                });

            return mock;
        }
    }
}
