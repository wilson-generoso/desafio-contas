using Desafio.Contas.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Desafio.Contas.Application.Command.UpdateAccount
{
    /// <summary>
    /// Comando CQRS para atualização de dados de conta
    /// </summary>
    public class UpdateAccountCommand : IRequestHandler<UpdateAccountRequest>
    {
        private readonly IAccountService accountService;
        private readonly ILogger<UpdateAccountCommand> logger;

        /// <summary>
        /// </summary>
        /// <param name="accountService">Serviço de infraestrutura para persistência de conta</param>
        /// <param name="logger"></param>
        public UpdateAccountCommand(IAccountService accountService, ILogger<UpdateAccountCommand> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        /// <summary>
        /// Executa a atualização de dados de conta
        /// </summary>
        /// <param name="request">Dados da conta a serem atualizados</param>
        /// <param name="cancellationToken"></param>
        public async Task<Unit> Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
        {
            using (logger.BeginScope($"Atualizando conta {request.Id}"))
            {
                var account = await accountService.Get(request.Id);

                account.Name = request.Name;
                account.Balance = request.Balance;

                await accountService.Update(account);

                logger.LogInformation($"Conta {account.Name} atualizada com sucesso.");

                return Unit.Value;
            }
        }
    }
}
