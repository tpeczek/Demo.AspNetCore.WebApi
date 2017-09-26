using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.AspNetCore.Mvc.CosmosDB.Services
{
    internal static class ServiceCollectionExtensions
    {
        private const string COSMOSDB_CONFIGURATION_SECTION = "CosmosDB";

        public static IServiceCollection AddCosmosDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosDBOptions>(configuration.GetSection(COSMOSDB_CONFIGURATION_SECTION));

            services.AddScoped<StarWarsCosmosDBClient>();

            return services;
        }
    }
}
