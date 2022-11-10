using Desafio.Contas.Application.Command.UpdateAccount;
using Desafio.Contas.Application.Test.Mocks;
using Desafio.Contas.Domain;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace Desafio.Contas.Application.Test.Commands
{
    public class UpdateAccountTests
    {
        [Fact]
        public async Task UpdateCorrectAccount()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<UpdateAccountCommand>();

            var accountService = new UpdateAccountServiceMock();

            var command = new UpdateAccountCommand(accountService, logger);

            var request = new UpdateAccountRequest
            {
                Id = AccountServiceMock.UpdateAccoundId,
                Name = "Teste",
                Balance = 100,
            };

            var response = await command.Handle(request, CancellationToken.None);

            response.ShouldBeOfType<MediatR.Unit>("Tipo de resposta inválida");

            var account = await accountService.Get(AccountServiceMock.UpdateAccoundId);

            account.ShouldNotBeNull("Conta não encontrada");
            account.Id.ShouldBe(request.Id);
            account.Name.ShouldBe(request.Name, "Nome não atualizado");
            account.Balance.ShouldBe(request.Balance);
        }

        [Fact]
        public async Task ValidateUpdateAccountNameNotNull()
        {
            var accountService = AccountServiceMock.BuildValidateAccountMock();

            var validator = new UpdateAccountRequestValidator(accountService.Object);

            var request = new UpdateAccountRequest
            {
                Id = AccountServiceMock.UpdateAccoundId,
                Name = ""
            };

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("Informe o nome da conta");
        }

        [Fact]
        public async Task ValidateUpdateAccountNameLength()
        {
            var accountService = AccountServiceMock.BuildValidateAccountMock();

            var validator = new UpdateAccountRequestValidator(accountService.Object);

            var request = new UpdateAccountRequest
            {
                Id = AccountServiceMock.UpdateAccoundId,
                Name = "".PadLeft(17, 'a')
            };

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome da conta deve possuir no máximo 16 caracteres");
        }

        [Fact]
        public async Task ValidateUpdateAccountIdNotExists()
        {
            var accountService = AccountServiceMock.BuildValidateAccountMock();

            var validator = new UpdateAccountRequestValidator(accountService.Object);

            var request = new UpdateAccountRequest
            {
                Id = Guid.NewGuid(),
                Name = "".PadLeft(17, 'a')
            };

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("Conta não cadastrada");
        }
    }
}