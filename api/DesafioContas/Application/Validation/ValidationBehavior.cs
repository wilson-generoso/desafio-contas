using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Contas.Application.Validation
{
    /// <summary>
    /// Objeto do Mediator para configurar o comportamento de validações do FluentValidation
    /// </summary>
    /// <typeparam name="TRequest">Objeto de solicitação ao Mediator</typeparam>
    /// <typeparam name="TResponse">Objeto de resposta da solicitação ao Mediator</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        /// <summary>
        /// </summary>
        /// <param name="validators">Validadores encontrados na solicitação do Mediator</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        /// <summary>
        /// Executa a validação da solicitação no FluentValidation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                if (!validationResults.IsValid(out var exception))
                    throw exception;
            }

            return await next();
        }
    }
}
