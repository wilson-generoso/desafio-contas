using Desafio.Contas.Application.Command.UpdateAccountBalance;
using Desafio.Contas.Domain;
using Desafio.Contas.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Desafio.Contas.Application.Command.AddEntry
{
    /// <summary>
    /// Comando CQRS para registro de novo lançamento em conta
    /// </summary>
    public class AddEntryCommand : IRequestHandler<AddEntryRequest>
    {
        private readonly IEntryService entryService;
        private readonly IAccountService accountService;
        private readonly IMediator mediator;
        private readonly ILogger<AddEntryCommand> logger;

        /// <summary>
        /// </summary>
        /// <param name="entryService">Serviço de infraestrutura para persistência de lançamentos</param>
        /// <param name="accountService">Serviço de infraestrutura para pesquisa de conta</param>
        /// <param name="mediator">Mediator do CQRS para submeter atualização de saldo na conta</param>
        /// <param name="logger"></param>
        public AddEntryCommand(IEntryService entryService, IAccountService accountService, IMediator mediator, ILogger<AddEntryCommand> logger)
        {
            this.entryService = entryService;
            this.accountService = accountService;
            this.mediator = mediator;
            this.logger = logger;
        }

        /// <summary>
        /// Executa o registro de lançamento em conta
        /// </summary>
        /// <param name="request">Dados de lançamento em conta</param>
        /// <param name="cancellationToken"></param>
        public async Task<Unit> Handle(AddEntryRequest request, CancellationToken cancellationToken)
        {
            using (logger.BeginScope($"Adicionando lançamento para conta {request.AccountId}"))
            {
                if (DateTime.TryParseExact(request.Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                {
                    var account = await accountService.Get(request.AccountId);

                    var entry = new Entry
                    {
                        Id = Guid.NewGuid(),
                        Account = account,
                        CreatedAt = DateTime.UtcNow,
                        Date = date,
                        Description = request.Description,
                        PreviousBalance = account.Balance,
                        Value = request.Value,
                        Type = (EntryType)request.Type
                    };

                    await entryService.Create(entry);

                    logger.LogInformation($"Lançamento {entry.Description} de valor {(entry.Type == EntryType.Debit ? entry.Value * -1: entry.Value)} registrado na conta {account.Name}");

                    await UpdateAccountBalance(entry, cancellationToken);
                }

                return Unit.Value;
            }
        }

        /// <summary>
        /// Solicita atualização de saldo da conta
        /// </summary>
        /// <param name="entry">Dados de lançamento</param>
        /// <param name="cancellationToken"></param>
        private async Task UpdateAccountBalance(Entry entry, CancellationToken cancellationToken)
        {
            using (logger.BeginScope($"Atualizando saldo da conta {entry.Account.Name}"))
            {
                var updateBalance = new UpdateAccountBalanceRequest
                {
                    Id = entry.Account.Id,
                    Value = entry.Type == EntryType.Debit ? entry.Value * -1 : entry.Value,
                };

                await mediator.Publish(updateBalance, cancellationToken);

                logger.LogInformation($"Atualização do saldo da conta {entry.Account.Name} enviada com sucesso");
            }
        }
    }
}
