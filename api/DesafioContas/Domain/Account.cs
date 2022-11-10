using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Contas.Domain
{
    /// <summary>
    /// Dados que representam uma conta
    /// </summary>
    public class Account : BaseEntity
    {
        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Data de registro da conta 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Saldo atual da conta
        /// </summary>
        public decimal Balance { get; set; }
    }
}
