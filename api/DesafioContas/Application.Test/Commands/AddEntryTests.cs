using Desafio.Contas.Application.Command.AddEntry;
using Desafio.Contas.Application.Command.UpdateAccountBalance;
using Desafio.Contas.Application.Test.Mocks;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace Desafio.Contas.Application.Test.Commands
{
    public class AddEntryTests
    {
        [Fact]
        public async Task AddCorrectEntry()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<AddEntryCommand>();

            var entryService = EntryServiceMock.BuildAddEntryServiceMock();
            var accountService = AccountServiceMock.BuildAccountServiceMock();

            var mediator = new Mock<IMediator>();

            mediator.Setup(x => x.Publish(It.IsAny<UpdateAccountBalanceRequest>(), CancellationToken.None)).Returns((UpdateAccountBalanceRequest a, CancellationToken b) => Task.CompletedTask);

            var command = new AddEntryCommand(entryService.Object, accountService.Object, mediator.Object, logger);

            var request = new AddEntryRequest
            {
                AccountId = AccountServiceMock.UpdateAccoundId,
                Description = "Entrada teste",
                Type = 1,
                Value = 10,
                Date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
            };

            var response = await command.Handle(request, CancellationToken.None);

            response.ShouldBeOfType<MediatR.Unit>("Tipo de resposta inválida");
        }

        [Fact]
        public async Task ValidateAddEntryNameNotNull()
        {
            var accountService = AccountServiceMock.BuildValidateAccountMock();

            var validator = new AddEntryRequestValidator(accountService.Object);

            var result = await validator.TestValidateAsync(new AddEntryRequest { AccountId = AccountServiceMock.UpdateAccoundId });

            result.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage("Informe a descrição do lançamento");
            result.ShouldHaveValidationErrorFor(x => x.Date).WithErrorMessage("Data do lançamento é invalida ou não foi informada (formato: AAAA-MM-DD)");
            result.ShouldHaveValidationErrorFor(x => x.Value).WithErrorMessage("O valor do lançamento deve ser maior que zero");
            result.ShouldHaveValidationErrorFor(x => x.Type).WithErrorMessage("Tipo de lançamento inválido ou não informado (1 = Débito ou 2 = Crédito).");
        }

        [Fact]
        public async Task ValidateAddEntryDateAfterNow()
        {
            var accountService = AccountServiceMock.BuildValidateAccountMock();

            var validator = new AddEntryRequestValidator(accountService.Object);

            var result = await validator.TestValidateAsync(new AddEntryRequest { AccountId = AccountServiceMock.UpdateAccoundId, Date = DateTime.Now.AddDays(1D).ToString("yyyy-MM-dd") });

            result.ShouldHaveValidationErrorFor(x => x.Date).WithErrorMessage("Data do lançamento deve ser menor ou igual a hoje");
        }

        [Fact]
        public async Task ValidateAddEntryAccountExists()
        {
            var accountService = AccountServiceMock.BuildValidateAccountMock();

            var validator = new AddEntryRequestValidator(accountService.Object);

            var result = await validator.TestValidateAsync(new AddEntryRequest { AccountId = Guid.NewGuid() });

            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("Conta não cadastrada");
        }
    }
}