namespace Desafio.Contas.Domain
{
    /// <summary>
    /// Dados que representam um lançamento em conta
    /// </summary>
    public class Entry : BaseEntity
    {
        /// <summary>
        /// Conta
        /// </summary>
        public Account Account { get; set; }
        /// <summary>
        /// Descrição do lançamento
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Data de registro do lançamento
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Saldo anterior no momento do lançamento
        /// </summary>
        public decimal PreviousBalance { get; set; }
        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Tipo de lançamento contábil
        /// </summary>
        public EntryType Type { get; set; }
    }

    /// <summary>
    /// Tipos de lançamentos contábeis
    /// </summary>
    public enum EntryType
    {
        /// <summary>
        /// Débito
        /// </summary>
        Debit = 1,
        /// <summary>
        /// Crédito
        /// </summary>
        Credit = 2  
    }
}
