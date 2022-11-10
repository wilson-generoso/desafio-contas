using FluentValidation;
using Desafio.Contas.Infrastructure;
using MediatR;

namespace Desafio.Contas.Application.Query.SearchEntries
{
    /// <summary>
    /// Dados de solicitação de pesquisa por lançamentos em conta
    /// </summary>
    public class SearchEntriesRequest : IRequest<SearchEntriesResponse>
    {
        /// <summary>
        /// Identificador da conta
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Quantidade de últimos dias nos quais serão pesquisados os lançamentos (não obrigatório) [Valor padrão = 7 dias]
        /// </summary>
        public int? LastDaysCount { get; set; } = 7;

        /// <summary>
        /// Data inicial de lançamentos em conta
        /// </summary>
        public string? InitialDate { get; set; } = null;

        /// <summary>
        /// Data final de lançamentos em conta
        /// </summary>
        public string? EndDate { get; set; } = null;
    }

    /// <summary>
    /// Validados dos dados de solicitação de pesquisa por lançamentos em conta
    /// </summary>
    public class SearchEntriesRequestValidator : AbstractValidator<SearchEntriesRequest>
    {
        private readonly IAccountService accountService;

        /// <summary>
        /// Configura regras de validação
        /// </summary>
        /// <param name="accountService"></param>
        public SearchEntriesRequestValidator(IAccountService accountService)
        {
            this.accountService = accountService;

            RuleFor(e => e.AccountId)
                .MustAsync(AccountExists).WithMessage("Conta não cadastrada");

            RuleFor(e => e)
                .Must(x => CastDate(x.InitialDate) != DateTime.MinValue || x.LastDaysCount.HasValue)
                .WithMessage("A data inicial está inválida ou não foi informada. Você pode utilizar o parâmetro dos ultimos dias para pesquisar os lançamentos.");
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
            if (DateTime.TryParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dateTime))
                return dateTime;
            else
                return DateTime.MinValue;
        }
    }
}
