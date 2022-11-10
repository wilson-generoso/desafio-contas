using Desafio.Contas.Application.Validation;
using Desafio.Contas.Domain;
using Desafio.Contas.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Desafio.Contas.Application.Query.SearchEntries
{
    /// <summary>
    /// Query CQRS para pesquisa de lançamentos de conta
    /// </summary>
    public class SearchEntriesQuery : IRequestHandler<SearchEntriesRequest, SearchEntriesResponse>
    {
        private readonly IEntryService entryService;
        private readonly IAccountService accountService;
        private readonly ILogger<SearchEntriesQuery> logger;

        /// <summary>
        /// </summary>
        /// <param name="entryService">Serviço de infraestrutura para pesquisa de lançamentos</param>
        /// <param name="accountService">Serviço de infraestrutura para pesquisa de conta</param>
        /// <param name="logger"></param>
        public SearchEntriesQuery(IEntryService entryService, IAccountService accountService, ILogger<SearchEntriesQuery> logger)
        {
            this.entryService = entryService;
            this.accountService = accountService;
            this.logger = logger;
        }

        /// <summary>
        /// Executa pesquisa de lançamentos em conta conforme filtro
        /// </summary>
        /// <param name="request">Dados de filtro de pesquisa por lançamentos em conta</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Dados de lançamentos em conta conforme filtro de pesquisa</returns>
        /// <exception cref="ValidationException">Exceção de validação disparada quando não é possível converter as datas de pesquisa</exception>
        public async Task<SearchEntriesResponse> Handle(SearchEntriesRequest request, CancellationToken cancellationToken)
        {
            using (logger.BeginScope($"Pesquisando lançamentos"))
            {
                if (string.IsNullOrEmpty(request.InitialDate) && request.LastDaysCount.HasValue)
                    request.InitialDate = DateTime.Now.Date.AddDays(request.LastDaysCount.Value * -1).ToString("yyyy-MM-dd");

                IsDateValid(request.EndDate, out var endDate);

                if (!IsDateValid(request.InitialDate, out var initialDate))
                    throw new ValidationException("Data inicial inválida", $"Não foi possível converter a data inicial {request.InitialDate}");

                var account = await accountService.Get(request.AccountId);

                var entries = await entryService.GetEntries(request.AccountId, initialDate.Value, endDate);

                if (entries.Any())
                {
                    logger.LogInformation($"Encontrados lançamentos para conta {request.AccountId} conforme filtros de pesquisa informados.");

                    // Obtem lista de datas com incidencia de lançamentos
                    var dates = entries.Select(e => e.Date.Date).Distinct();

                    // Configura a resposta por dia
                    var days = dates.Select(date => GetDays(date, entries)).ToArray();

                    return new SearchEntriesResponse(new(account.Name, account.Balance), days);
                }
                else
                {
                    logger.LogInformation($"Não foram encontrados lançamentos para conta {request.AccountId} conforme filtro de pesquisa informado");
                    return new SearchEntriesResponse(new(account.Name, account.Balance), new DayEntriesResponse[0]);
                }
            }
        }

        /// <summary>
        /// Configura a resposta por dia de lançamentos em conta
        /// </summary>
        /// <param name="date">Data do lançamento</param>
        /// <param name="entries">Lista de lançamentos encontrados na pesquisa</param>
        /// <returns>Lista de resposta por dia</returns>
        private DayEntriesResponse GetDays(DateTime date, IEnumerable<Entry> entries)
        {
            // Obtem lista de lançamentos no dia
            var dayEntries = entries.Where(e => e.Date.Date == date).OrderBy(e => e.CreatedAt);

            // Pega o primeiro lançamento para calcular o saldo inicial do dia
            var firstEntry = dayEntries.First();

            // Pega ultimo lançamento para calcular saldo final do dia
            var lastEntry = dayEntries.Last();

            return new DayEntriesResponse(
                date,
                firstEntry.PreviousBalance,
                lastEntry.PreviousBalance + (lastEntry.Type == EntryType.Debit ? lastEntry.Value * -1 : lastEntry.Value), 
                dayEntries.Select(GetEntryResponse).ToArray()
            );
        }

        /// <summary>
        /// Produz resposta de lançamento em conta
        /// </summary>
        /// <param name="entry">Lançamento em conta</param>
        /// <returns>Resposta de lançamento em conta</returns>
        private SearchEntryResponse GetEntryResponse(Entry entry)
        {
            return new SearchEntryResponse(
                entry.Description,
                entry.Value,
                entry.Type == EntryType.Debit ? "Débito" : "Crédito"
            );
        }

        /// <summary>
        /// Verifica se a data é válida conforme 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateCasted"></param>
        /// <returns></returns>
        private bool IsDateValid(string date, out DateTime? dateCasted)
        {
            dateCasted = null;

            if (!string.IsNullOrEmpty(date) && DateTime.TryParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var outDate))
            {
                dateCasted = outDate;
                return true;
            }
            else
                return false;
        }
    }
}
