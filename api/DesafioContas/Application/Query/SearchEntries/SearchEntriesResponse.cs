namespace Desafio.Contas.Application.Query.SearchEntries
{
    /// <summary>
    /// Dados de resposta para solicitação de pesquisa por lançamentos em conta
    /// </summary>
    /// <param name="Account">Dados da conta pesquisada</param>
    /// <param name="Days">Dias de lançamentos em conta</param>
    public record SearchEntriesResponse(SearchEntryAccountResponse Account, DayEntriesResponse[] Days)
    {
    }

    /// <summary>
    /// Dados consolidados do dia de lançamentos em conta
    /// </summary>
    /// <param name="Date">Data do lançamento em conta</param>
    /// <param name="DayInitialBalance">Saldo inicial do dia</param>
    /// <param name="DayFinalBalance">Saldo final do dia</param>
    /// <param name="Entries">Registros de lançamentos em conta do dia</param>
    public record DayEntriesResponse(DateTime Date, decimal DayInitialBalance, decimal DayFinalBalance, SearchEntryResponse[] Entries)
    {
    }

    /// <summary>
    /// Dados de lançamento do dia
    /// </summary>
    /// <param name="Description">Descrição do lançamento</param>
    /// <param name="Value">Valor do lançamento</param>
    /// <param name="Type">Tipo de lançamento contábil</param>
    public record SearchEntryResponse(string Description, decimal Value, string Type)
    {
    }

    /// <summary>
    /// Dados da conta pesquisada
    /// </summary>
    /// <param name="Name">Nome da conta</param>
    /// <param name="Balance">Saldo atual</param>
    public record SearchEntryAccountResponse(string Name, decimal Balance)
    {
    }
}
