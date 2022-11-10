using FluentValidation;
using Desafio.Contas.Infrastructure;
using MediatR;

namespace Desafio.Contas.Application.Command.AddAccount
{
    /// <summary>
    /// Dados de solicitação para operação de registro de nova conta
    /// </summary>
    public class AddAccountRequest : IRequest<AddAccountResponse>
    {
        /// <summary>
        /// Nome da conta (máximo de 16 caracteres)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Saldo da conta no momento do registro da nova conta
        /// </summary>
        public decimal Balance { get; set; }
    }

    /// <summary>
    /// Validador de dados da solicitação de registro da nova conta
    /// </summary>
    public class AddAccountRequestValidator : AbstractValidator<AddAccountRequest>
    {
        private readonly IAccountService accountService;

        /// <summary>
        /// Configura as regras de validação
        /// </summary>
        /// <param name="accountService"></param>
        public AddAccountRequestValidator(IAccountService accountService)
        {
            this.accountService = accountService;

            RuleFor(a => a.Name)
                .NotEmpty()
                .NotNull().WithMessage("Informe o nome da conta")
                .MaximumLength(16).WithMessage("O nome da conta deve possuir no máximo 16 caracteres")
                .MustAsync(IsUniqueAccountName).WithMessage("O nome da conta já existe");
        }

        /// <summary>
        /// Verifica se existe uma conta registrada com o mesmo nome
        /// </summary>
        /// <param name="accountName">Nome da conta</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Identificação se existe ou não uma conta com o mesmo nome</returns>
        private async Task<bool> IsUniqueAccountName(string accountName, CancellationToken cancellationToken)
        {
            return !await this.accountService.HasAccountName(accountName);
        }
    }
}
