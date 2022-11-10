using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Contas.Application.Validation
{
    /// <summary>
    /// Objeto extensão do objeto ValidationResult para validações e produção de exceções de validação
    /// </summary>
    public static class ValidationExceptionExtension
    {
        /// <summary>
        /// Valida se o resultado da validação foi com sucesso. Caso negativo, é produzido objeto de exceção de validação
        /// </summary>
        /// <param name="results">Lista de resultados de validação</param>
        /// <param name="exception">Exceção de validação produzida caso insucesso</param>
        /// <returns>Indica se está valido ou não</returns>
        public static bool IsValid(this IEnumerable<ValidationResult> results, out ValidationException exception)
        {
            var isValid = true;
            exception = null;

            if (results != null && results.Any(r => !r.IsValid && r.Errors.Any(e => !string.IsNullOrEmpty(e?.ErrorMessage))))
            {
                exception = new ValidationException(results);
                isValid = false;
            }

            return isValid;
        }
    }
}
