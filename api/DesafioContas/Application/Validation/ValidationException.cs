using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Contas.Application.Validation
{
    /// <summary>
    /// Exceção de validação utilizada pelo FluentValidation
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// </summary>
        /// <param name="message">Mensagem geral para o solicitante</param>
        /// <param name="errors">Lista de falhas de validação</param>
        public ValidationException(string message, params string[] errors)
            : base(message)
        {
            this.Errors = new List<string>(errors);
        }

        /// <summary>
        /// </summary>
        /// <param name="validationResults">Lista de resultados de validação do FluentValidation</param>
        public ValidationException(IEnumerable<ValidationResult> validationResults)
            : base("Ocorreram erros de validação para sua solicitação")
        {
            this.Errors = new List<string>();
            validationResults
                .Where(vr => !vr.IsValid && vr.Errors.Any(e => !string.IsNullOrEmpty(e?.ErrorMessage)))
                .SelectMany(vr => vr.Errors.Select(vf => vf.ErrorMessage))
                .ToList()
                .ForEach(e => this.Errors.Add(e));
        }

        /// <summary>
        /// Lista de falhas de validação
        /// </summary>
        public List<string> Errors { get; }
    }
}
