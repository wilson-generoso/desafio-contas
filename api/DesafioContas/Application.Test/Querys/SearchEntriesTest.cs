using Desafio.Contas.Application.Command.UpdateAccount;
using Desafio.Contas.Application.Query.SearchEntries;
using Desafio.Contas.Application.Test.Mocks;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace Desafio.Contas.Application.Test.Querys
{
    public class SearchEntriesTest
    {
        [Fact]
        public async Task GetAllAccountEntries()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SearchEntriesQuery>();

            var accountService = AccountServiceMock.BuildAccountServiceMock();
            var entryService = EntryServiceMock.BuildSearchFeirasServiceMock(accountService.Object, AccountServiceMock.UpdateAccoundId);

            var query = new SearchEntriesQuery(entryService.Object, accountService.Object, logger);

            var request = new SearchEntriesRequest
            {
                AccountId = AccountServiceMock.UpdateAccoundId,
                LastDaysCount = 60
            };

            var response = await query.Handle(request, CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeOfType<SearchEntriesResponse>();
            response?.Account.ShouldNotBeNull();
            response?.Days.ShouldNotBeNull().Count().ShouldBeEquivalentTo(6);
        }

        [Fact]
        public async Task GetAccountEntriesLast10Days()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SearchEntriesQuery>();

            var accountService = AccountServiceMock.BuildAccountServiceMock();
            var entryService = EntryServiceMock.BuildSearchFeirasServiceMock(accountService.Object, AccountServiceMock.UpdateAccoundId);

            var query = new SearchEntriesQuery(entryService.Object, accountService.Object, logger);

            var request = new SearchEntriesRequest
            {
                AccountId = AccountServiceMock.UpdateAccoundId,
                LastDaysCount = 10
            };

            var response = await query.Handle(request, CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeOfType<SearchEntriesResponse>();
            response?.Account.ShouldNotBeNull();
            response?.Days.ShouldNotBeNull().Count().ShouldBeEquivalentTo(2);
            response?.Days[0].Entries.Count().ShouldBeEquivalentTo(3);
            response?.Days[1].Entries.Count().ShouldBeEquivalentTo(2);
        }

        [Fact]
        public async Task GetAccountEntriesInitialDateOnly()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SearchEntriesQuery>();

            var accountService = AccountServiceMock.BuildAccountServiceMock();
            var entryService = EntryServiceMock.BuildSearchFeirasServiceMock(accountService.Object, AccountServiceMock.UpdateAccoundId);

            var query = new SearchEntriesQuery(entryService.Object, accountService.Object, logger);

            var request = new SearchEntriesRequest
            {
                AccountId = AccountServiceMock.UpdateAccoundId,
                InitialDate = DateTime.Now.Date.AddDays(-30).ToString("yyyy-MM-dd")
            };

            var response = await query.Handle(request, CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeOfType<SearchEntriesResponse>();
            response?.Account.ShouldNotBeNull();
            response?.Days.ShouldNotBeNull().Count().ShouldBeEquivalentTo(4);
            response?.Days[0].Entries.Count().ShouldBeEquivalentTo(1);
            response?.Days[1].Entries.Count().ShouldBeEquivalentTo(1);
            response?.Days[2].Entries.Count().ShouldBeEquivalentTo(3);
            response?.Days[3].Entries.Count().ShouldBeEquivalentTo(2);
        }

        [Fact]
        public async Task GetAccountEntriesRangeDate()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SearchEntriesQuery>();

            var accountService = AccountServiceMock.BuildAccountServiceMock();
            var entryService = EntryServiceMock.BuildSearchFeirasServiceMock(accountService.Object, AccountServiceMock.UpdateAccoundId);

            var query = new SearchEntriesQuery(entryService.Object, accountService.Object, logger);

            var request = new SearchEntriesRequest
            {
                AccountId = AccountServiceMock.UpdateAccoundId,
                InitialDate = DateTime.Now.Date.AddDays(-30).ToString("yyyy-MM-dd"),
                EndDate = DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd")
            };

            var response = await query.Handle(request, CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeOfType<SearchEntriesResponse>();
            response?.Account.ShouldNotBeNull();
            response?.Days.ShouldNotBeNull().Count().ShouldBeEquivalentTo(2);
            response?.Days[0].Entries.Count().ShouldBeEquivalentTo(1);
            response?.Days[1].Entries.Count().ShouldBeEquivalentTo(1);
        }

        [Fact]
        public async Task GetAccountEntriesNotFound()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SearchEntriesQuery>();

            var accountService = AccountServiceMock.BuildAccountServiceMock();
            var entryService = EntryServiceMock.BuildSearchFeirasServiceMock(accountService.Object, AccountServiceMock.UpdateAccoundId);

            var query = new SearchEntriesQuery(entryService.Object, accountService.Object, logger);

            var request = new SearchEntriesRequest
            {
                AccountId = Guid.NewGuid(),
                InitialDate = DateTime.Now.Date.AddDays(-30).ToString("yyyy-MM-dd"),
                EndDate = DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd")
            };

            var response = await query.Handle(request, CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeOfType<SearchEntriesResponse>();
            response?.Account.ShouldNotBeNull();
            response?.Days.ShouldNotBeNull().Count().ShouldBeEquivalentTo(0);
        }

        [Fact]
        public async Task GetAccountEntriesInvalidInitialDate()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SearchEntriesQuery>();

            var accountService = AccountServiceMock.BuildAccountServiceMock();
            var entryService = EntryServiceMock.BuildSearchFeirasServiceMock(accountService.Object, AccountServiceMock.UpdateAccoundId);

            var query = new SearchEntriesQuery(entryService.Object, accountService.Object, logger);

            var exception = await Should.ThrowAsync<Application.Validation.ValidationException>(async () =>
            {
                var request = new SearchEntriesRequest
                {
                    AccountId = Guid.NewGuid(),
                    InitialDate = "2022-02-30"
                };

                await query.Handle(request, CancellationToken.None);
            });

            exception.Should().NotBeNull();
            exception.Should().BeOfType<Application.Validation.ValidationException>();
            exception.Message.ShouldBe("Data inicial inválida");
            exception?.Errors?.Count.Should().Be(1);
            exception?.Errors?[0].ShouldBe("Não foi possível converter a data inicial 2022-02-30");
        }

        [Fact]
        public async Task ValidateSearchEntryAccountIdNotExists()
        {
            var accountService = AccountServiceMock.BuildValidateAccountMock();

            var validator = new SearchEntriesRequestValidator(accountService.Object);

            var request = new SearchEntriesRequest
            {
                AccountId = Guid.NewGuid()
            };

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("Conta não cadastrada");
        }
    }
}
