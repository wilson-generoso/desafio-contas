using Desafio.Contas.Domain;
using Desafio.Contas.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Desafio.Contas.Application.Query.GetAccounts
{
    /// <summary>
    /// Query CQRS para pesquisa de contas cadastradas
    /// </summary>
    public class GetAccountsQuery : IRequestHandler<GetAccountsRequest, GetAccountsResponse>
    {
        private readonly IAccountService accountService;
        private readonly ILogger<GetAccountsQuery> logger;

        /// <summary>
        /// </summary>
        /// <param name="accountService">Serviço de infraestrutura para pesquisa por contas</param>
        /// <param name="logger"></param>
        public GetAccountsQuery(IAccountService accountService, ILogger<GetAccountsQuery> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        /// <summary>
        /// Executa a pesquisa de contas conforme filtro
        /// </summary>
        /// <param name="request">Dados de filtro de pesquisa por contas cadastradas</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Lista de contas cadastradas encontradas conforme critério de filtro</returns>
        public async Task<GetAccountsResponse> Handle(GetAccountsRequest request, CancellationToken cancellationToken)
        {
            using (logger.BeginScope($"Obtendo contas"))
            {
                var accounts = new List<Account>();

                if (request?.Id.HasValue ?? false)
                {
                    var account = await accountService.Get(request.Id.Value);

                    if (account != null)
                    {
                        accounts.Add(account);
                        logger.LogInformation($"Conta {request.Id.Value} encontrada.");
                    }
                    else
                        logger.LogInformation($"Não foram encontradas contas para o filtro informado (Id = {request.Id.Value}).");
                }
                else
                {
                    accounts.AddRange(await accountService.GetAll());

                    logger.LogInformation($"Pesquisa sem filtro. Listada todas as contas cadastradas.");
                }

                return new GetAccountsResponse(accounts.ToArray());
            }
        }
    }
}
