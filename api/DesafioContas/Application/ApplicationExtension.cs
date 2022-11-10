using FluentValidation;
using Desafio.Contas.Application.Validation;
using MediatR;
using System.Reflection;
using Desafio.Contas.Application.Query.GetAccounts;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Objeto de extensão para configuração da camada de aplicação
    /// </summary>
    public static class ApplicationExtension
    {
        /// <summary>
        /// Adiciona configurações da aplicação na inicialização
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
