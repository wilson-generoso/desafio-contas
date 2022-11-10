using FluentValidation;
using Desafio.Contas.Infrastructure;
using MediatR;

namespace Desafio.Contas.Application.Command.UpdateAccount
{
    /// <summary>
    /// Dados de solicitação de atualização de dados de conta
    /// </summary>
    public class UpdateAccountRequest : IRequest
    {
        /// <summary>
        /// Identificador da conta
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Nome da conta 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Saldo no momento da atualização
        /// </summary>
        public decimal Balance { get; set; }
    }

    /// <summary>
    /// Validador dos dados de solicitação para atualização de dados de conta
    /// </summary>
    public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
    {
        private readonly IAccountService accountService;

        /// <summary>
        /// Configura regras de validação
        /// </summary>
        /// <param name="accountService">Serviço de infraestrutura para pesquisa de conta</param>
        public UpdateAccountRequestValidator(IAccountService accountService)
        {
            this.accountService = accountService;

            RuleFor(a => a.Id)
                .MustAsync(AccountExists).WithMessage("Conta não cadastrada");

            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Informe o nome da conta")
                .NotNull().WithMessage("Informe o nome da conta")
                .MaximumLength(16).WithMessage("O nome da conta deve possuir no máximo 16 caracteres");
        }

        /// <summary>
        /// Verifica se a conta existe
        /// </summary>
        /// <param name="accountId">Identificador da conta</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> AccountExists(Guid accountId, CancellationToken cancellationToken)
        {
            return await this.accountService.Exists(accountId);
        }
    }
}
