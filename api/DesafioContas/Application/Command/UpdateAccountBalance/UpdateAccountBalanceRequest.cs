using MediatR;

namespace Desafio.Contas.Application.Command.UpdateAccountBalance
{
    /// <summary>
    /// Dados da solicitação de atualização de saldo de conta
    /// </summary>
    public class UpdateAccountBalanceRequest : INotification
    {
        /// <summary>
        /// Identificador da conta
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Valor a ser adicionado ao saldo da conta
        /// </summary>
        public decimal Value { get; set; }
    }
}
