using Desafio.Contas.Application.Command.UpdateAccountBalance;
using Desafio.Contas.Application.Test.Mocks;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace Desafio.Contas.Application.Test.Commands
{
    public class UpdateAccountBalanceTest
    {
        [Fact]
        public async Task UpdateAccountBalanceWithDebit()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<UpdateAccountBalanceCommand>();

            var accountService = new UpdateAccountServiceMock();

            var command = new UpdateAccountBalanceCommand(accountService, logger);

            var request = new UpdateAccountBalanceRequest
            {
                Id = AccountServiceMock.UpdateAccoundId,
                Value = -50m
            };

            var balance = (await accountService.Get(AccountServiceMock.UpdateAccoundId))?.Balance;

            await command.Handle(request, CancellationToken.None);

            var account = await accountService.Get(AccountServiceMock.UpdateAccoundId);

            account.ShouldNotBeNull();
            account?.Balance.ShouldBeEquivalentTo(balance - 50m, "Contabilização errada para débito");
        }

        [Fact]
        public async Task UpdateAccountBalanceWithCredit()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<UpdateAccountBalanceCommand>();

            var accountService = new UpdateAccountServiceMock();

            var command = new UpdateAccountBalanceCommand(accountService, logger);

            var request = new UpdateAccountBalanceRequest
            {
                Id = AccountServiceMock.UpdateAccoundId,
                Value = 50m
            };

            var balance = (await accountService.Get(AccountServiceMock.UpdateAccoundId))?.Balance;

            await command.Handle(request, CancellationToken.None);

            var account = await accountService.Get(AccountServiceMock.UpdateAccoundId);

            account.ShouldNotBeNull();
            account?.Balance.ShouldBeEquivalentTo(balance + 50m, "Contabilização errada para débito");
        }

        [Fact]
        public async Task UpdateAccountBalanceWithUpdateFail()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<UpdateAccountBalanceCommand>();

            var accountService = new UpdateFailAccountServiceMock();

            var command = new UpdateAccountBalanceCommand(accountService, logger);

            var request = new UpdateAccountBalanceRequest
            {
                Id = AccountServiceMock.UpdateAccoundId,
                Value = 50m
            };

            var balance = (await accountService.Get(AccountServiceMock.UpdateAccoundId))?.Balance;

            await command.Handle(request, CancellationToken.None);

            var account = await accountService.Get(AccountServiceMock.UpdateAccoundId);

            account.ShouldNotBeNull();
            account?.Balance.ShouldBeEquivalentTo(balance + 50m, "Contabilização errada para débito");
        }
    }
}
