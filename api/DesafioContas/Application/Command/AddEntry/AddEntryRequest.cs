using FluentValidation;
using Desafio.Contas.Infrastructure;
using MediatR;

namespace Desafio.Contas.Application.Command.AddEntry
{
    /// <summary>
    /// Dados da solicitação de registro de lançamento em conta
    /// </summary>
    public class AddEntryRequest : IRequest
    {
        /// <summary>
        /// Identificador da conta
        /// </summary>
        public Guid AccountId { get; set; }
        /// <summary>
        /// Descrição do lançamento
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Data do lançamento
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Tipo de lançamento contábil (1 = Debito e 2 = Credito)
        /// </summary>
        public int Type { get; set; }
    }

    /// <summary>
    /// Validador dos dados de solicitação de registro de lançamento em conta
    /// </summary>
    public class AddEntryRequestValidator : AbstractValidator<AddEntryRequest>
    {
        private readonly IAccountService accountService;

        /// <summary>
        /// Configura regras de validação
        /// </summary>
        /// <param name="accountService">Serviço de infraestrutura para pesquisa de conta</param>
        public AddEntryRequestValidator(IAccountService accountService)
        {
            this.accountService = accountService;

            RuleFor(e => e.AccountId)
                .MustAsync(AccountExists).WithMessage("Conta não cadastrada");

            RuleFor(e => e.Description)
                .NotEmpty().WithMessage("Informe a descrição do lançamento")
                .NotNull().WithMessage("Informe a descrição do lançamento");

            RuleFor(e => e.Date)
                .Must(date => CastDate(date) != DateTime.MinValue).WithMessage("Data do lançamento é invalida ou não foi informada (formato: AAAA-MM-DD)")
                .Must(date => CastDate(date) <= DateTime.Now.Date).WithMessage("Data do lançamento deve ser menor ou igual a hoje");

            RuleFor(e => e.Value)
                .GreaterThan(0m).WithMessage("O valor do lançamento deve ser maior que zero");

            RuleFor(e => e.Type)
                .InclusiveBetween(1, 2).WithMessage("Tipo de lançamento inválido ou não informado (1 = Débito ou 2 = Crédito).");
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

        /// <summary>
        /// Converte a data em formato AAAA-MM-DD para datetime
        /// </summary>
        /// <param name="date">Data em formato AAAA-MM-DD</param>
        /// <returns>Data convertida</returns>
        private DateTime CastDate(string date)
        {
            if(DateTime.TryParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dateTime))
                return dateTime;
            else
                return DateTime.MinValue;
        }
    }
}
