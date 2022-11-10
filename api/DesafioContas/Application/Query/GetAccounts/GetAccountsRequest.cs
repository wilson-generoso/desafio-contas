using MediatR;

namespace Desafio.Contas.Application.Query.GetAccounts
{
    /// <summary>
    /// Dados de solicitação para pesquisa de contas
    /// </summary>
    public class GetAccountsRequest : IRequest<GetAccountsResponse>
    {
        /// <summary>
        /// Identificador da conta
        /// </summary>
        public Guid? Id { get; set; }
    }
}
