using Desafio.Contas.Domain;

namespace Desafio.Contas.Application.Query.GetAccounts
{
    /// <summary>
    /// Dados de resposta para pesquisa por contas cadastradas
    /// </summary>
    /// <param name="Accounts">Lista de contas encontradas</param>
    public record GetAccountsResponse(Account[] Accounts)
    {
    }
}
