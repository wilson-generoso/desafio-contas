using Desafio.Contas.Infrastructure;
using Desafio.Contas.Infrastructure.MongoDb.Account;
using Desafio.Contas.Infrastructure.MongoDb.Entry;
using Desafio.Contas.Infrastructure.MongoDb.Repository;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureMongoDBExtension
    {
        public static IServiceCollection AddInfrastructureMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

#pragma warning disable CS0618 // O tipo ou membro é obsoleto
            MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;
#pragma warning restore CS0618 // O tipo ou membro é obsoleto

            var settings = GetMongoDBSettings();

            if (settings == null)
            {
                settings = new MongoDBSettings();
                configuration.GetSection(MongoDBSettings.CONFIG_KEY).Bind(settings);
            }

            services.AddSingleton(settings);
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IEntryService, EntryService>();

            return services;
        }

        private static MongoDBSettings GetMongoDBSettings()
        {
            var settings = Environment.GetEnvironmentVariable("MONGODB_SETTINGS");

            if (string.IsNullOrEmpty(settings))
                return null;
            else
                return JsonSerializer.Deserialize<MongoDBSettings>(settings);
        }
    }
}
