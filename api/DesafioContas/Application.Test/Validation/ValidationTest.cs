using Desafio.Contas.Application.Command.UpdateAccount;
using Desafio.Contas.Application.Test.Mocks;
using Desafio.Contas.Application.Validation;
using FluentAssertions;
using FluentValidation;
using Moq;
using Shouldly;

namespace Desafio.Contas.Application.Test.Validation
{
    public class ValidationTest
    {
        [Fact(DisplayName = "Testa o comportamento do middleware de validação com excecao")]
        public async Task ExecuteBehaviorHandleException()
        {
            var service = AccountServiceMock.BuildValidateAccountMock();

            var validator = new UpdateAccountRequestValidator(service.Object);

            var behavior = new ValidationBehavior<UpdateAccountRequest, MediatR.Unit>(new IValidator<UpdateAccountRequest>[] { validator.As<IValidator<UpdateAccountRequest>>() });

            var exception = await Should.ThrowAsync<Application.Validation.ValidationException>(async () =>
            {
                var request = new UpdateAccountRequest { Id = Guid.NewGuid(), Name = "aaa" };
                var requestHandler = new Mock<MediatR.RequestHandlerDelegate<MediatR.Unit>>();
                requestHandler.Setup(x => x.Invoke()).Returns(() => Task.FromResult(MediatR.Unit.Value));

                await behavior.Handle(request, requestHandler.Object, CancellationToken.None);
            });

            exception.Should().NotBeNull();
            exception.Should().BeOfType<Application.Validation.ValidationException>();
            exception?.Errors?.Count.Should().Be(1);
        }

        [Fact(DisplayName = "Testa o comportamento do middleware de validação sem excecao")]
        public async Task ExecuteBehaviorHandleSuccess()
        {
            var service = AccountServiceMock.BuildValidateAccountMock();

            var validator = new UpdateAccountRequestValidator(service.Object);

            var behavior = new ValidationBehavior<UpdateAccountRequest, MediatR.Unit>(new IValidator<UpdateAccountRequest>[] { validator.As<IValidator<UpdateAccountRequest>>() });

            var request = new UpdateAccountRequest { Id = AccountServiceMock.UpdateAccoundId, Name = "aaa" };
            var requestHandler = new Mock<MediatR.RequestHandlerDelegate<MediatR.Unit>>();
            requestHandler.Setup(x => x.Invoke()).Returns(() => Task.FromResult(MediatR.Unit.Value));

            var result = await behavior.Handle(request, requestHandler.Object, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeOfType<MediatR.Unit>();
        }

        [Fact(DisplayName = "Testa a excecao ValidationException")]
        public Task ValidationExceptionSimple()
        {
            var exception = new Application.Validation.ValidationException("Mensagem de validação", "Erro 1");

            exception?.Message.Should().BeEquivalentTo("Mensagem de validação");
            exception?.Errors?.Count.Should().Be(1);

            return Task.CompletedTask;
        }
    }
}
