using Desafio.Contas.Application.Command.UpdateAccount;
using Desafio.Contas.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Desafio.Contas.Application.Command.UpdateAccountBalance
{
    /// <summary>
    /// Comando CQRS para atualização de saldo de conta
    /// </summary>
    public class UpdateAccountBalanceCommand : INotificationHandler<UpdateAccountBalanceRequest>
    {
        private readonly IAccountService accountService;
        private readonly ILogger<UpdateAccountCommand> logger;

        /// <summary>
        /// </summary>
        /// <param name="accountService">Serviço de infraestrutura para persistência de conta</param>
        /// <param name="logger"></param>
        public UpdateAccountBalanceCommand(IAccountService accountService, ILogger<UpdateAccountCommand> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        /// <summary>
        /// Executa a atualização do saldo da conta
        /// </summary>
        /// <param name="request">Dados para atualização de saldo de conta</param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(UpdateAccountBalanceRequest request, CancellationToken cancellationToken)
        {
            using (logger.BeginScope($"Atualizando o saldo da conta {request.Id}"))
            {
                try
                {
                    var account = await accountService.Get(request.Id);

                    account.Balance += request.Value;

                    await accountService.Update(account);

                    logger.LogInformation($"O saldo da conta {account.Name} atualizado com sucesso");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao atualizar o saldo da conta {0} com o valor {1}", request.Id, request.Value);
                }
            }
        }
    }
}
