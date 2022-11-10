using Desafio.Contas.Application.Command.AddAccount;
using Desafio.Contas.Application.Test.Mocks;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace Desafio.Contas.Application.Test.Commands
{
    public class AddAccountTests
    {
        [Fact]
        public async Task AddCorrectAccount()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<AddAccountCommand>();

            var accountService = AccountServiceMock.BuildAddAccountServiceMock();

            var command = new AddAccountCommand(accountService.Object, logger);

            var request = new AddAccountRequest
            {
                Name = "Teste",
                Balance = 100
            };

            var response = await command.Handle(request, CancellationToken.None);

            response
                .ShouldBeOfType<AddAccountResponse>("Tipo de resposta inválida")
                .ShouldNotBeNull("Resposta não pode ser nula")
                .Id.ShouldNotBe(Guid.Empty, "Não foi gerado ID");
        }

        [Fact]
        public async Task ValidateAddAccountNameNotNull()
        {
            var accountService = AccountServiceMock.BuildValidateAddAccountMock();

            var validator = new AddAccountRequestValidator(accountService.Object);

            var result = await validator.TestValidateAsync(new AddAccountRequest());

            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("Informe o nome da conta");
        }

        [Fact]
        public async Task ValidateAddAccountNameLength()
        {
            var accountService = AccountServiceMock.BuildValidateAddAccountMock();

            var validator = new AddAccountRequestValidator(accountService.Object);

            var result = await validator.TestValidateAsync(new AddAccountRequest { Name = "".PadRight(17, 'a') });

            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome da conta deve possuir no máximo 16 caracteres");
        }

        [Fact]
        public async Task ValidateAddAccountNameExists()
        {
            var accountService = AccountServiceMock.BuildValidateAddAccountMock();

            var validator = new AddAccountRequestValidator(accountService.Object);

            var result = await validator.TestValidateAsync(new AddAccountRequest { Name = "ValidateAddAccount" });

            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome da conta já existe");
        }
    }
}