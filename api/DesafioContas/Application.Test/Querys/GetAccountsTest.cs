using Desafio.Contas.Application.Query.GetAccounts;
using Desafio.Contas.Application.Test.Mocks;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace Desafio.Contas.Application.Test.Querys
{
    public class GetAccountsTest
    {

        [Fact]
        public async Task GetAllAccounts()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<GetAccountsQuery>();

            var accountService = AccountServiceMock.BuildGetAccountsServiceMock();

            var query = new GetAccountsQuery(accountService.Object, logger);

            var response = await query.Handle(new GetAccountsRequest(), CancellationToken.None);

            response.ShouldBeOfType<GetAccountsResponse>();
            response.ShouldNotBeNull();
            response?.Accounts.ShouldNotBeNull();
            response?.Accounts?.Count().ShouldBeGreaterThan(1);
        }

        [Fact]
        public async Task GetAccountById()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<GetAccountsQuery>();

            var accountService = AccountServiceMock.BuildGetAccountsServiceMock();

            var query = new GetAccountsQuery(accountService.Object, logger);

            var response = await query.Handle(new GetAccountsRequest {  Id = AccountServiceMock.UpdateAccoundId }, CancellationToken.None);

            response.ShouldBeOfType<GetAccountsResponse>();
            response.ShouldNotBeNull();
            response?.Accounts.ShouldNotBeNull();
            response?.Accounts?.Count().ShouldBeEquivalentTo(1);
            response?.Accounts[0].Id.ShouldBe(AccountServiceMock.UpdateAccoundId);
        }

        [Fact]
        public async Task GetAccountNotFound()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<GetAccountsQuery>();

            var accountService = AccountServiceMock.BuildGetAccountsServiceMock();

            var query = new GetAccountsQuery(accountService.Object, logger);

            var response = await query.Handle(new GetAccountsRequest { Id = Guid.NewGuid() }, CancellationToken.None);

            response.ShouldBeOfType<GetAccountsResponse>();
            response.ShouldNotBeNull();
            response?.Accounts.ShouldNotBeNull();
            response?.Accounts?.Count().ShouldBeEquivalentTo(0);
        }
    }
}
