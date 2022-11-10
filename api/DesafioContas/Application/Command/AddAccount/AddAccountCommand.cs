using Desafio.Contas.Domain;
using Desafio.Contas.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Desafio.Contas.Application.Command.AddAccount
{
    /// <summary>
    /// Comando CQRS para registro de nova conta
    /// </summary>
    public class AddAccountCommand : IRequestHandler<AddAccountRequest, AddAccountResponse>
    {
        private readonly IAccountService accountService;
        private readonly ILogger<AddAccountCommand> logger;

        /// <summary>
        /// </summary>
        /// <param name="accountService">Serviço de infraestrutura para persistência da conta</param>
        /// <param name="logger"></param>
        public AddAccountCommand(IAccountService accountService, ILogger<AddAccountCommand> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        /// <summary>
        /// Executa o registro de nova conta no serviço de infraestrutura
        /// </summary>
        /// <param name="request">Dados da solicitação para registro de nova conta</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AddAccountResponse> Handle(AddAccountRequest request, CancellationToken cancellationToken)
        {
            using (logger.BeginScope("Adicionando nova conta"))
            {
                var account = new Account()
                {
                    Id = Guid.NewGuid(),
                    Balance = request.Balance,
                    CreatedAt = DateTime.UtcNow,
                    Name = request.Name
                };

                await accountService.Create(account);

                logger.LogInformation($"Conta {account.Name} criada com sucesso.");

                return new AddAccountResponse(account.Id);
            }
        }
    }
}
