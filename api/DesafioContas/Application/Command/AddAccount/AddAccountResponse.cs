namespace Desafio.Contas.Application.Command.AddAccount
{
    /// <summary>
    /// Dados de resposta da solicitação de registro de nova conta
    /// </summary>
    /// <param name="Id">Identificador gerado no registro de nova conta</param>
    public record AddAccountResponse(Guid Id)
    {
    }
}
