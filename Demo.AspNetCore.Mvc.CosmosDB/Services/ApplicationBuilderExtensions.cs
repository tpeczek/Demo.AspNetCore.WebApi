using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.AspNetCore.Mvc.CosmosDB.Services
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCosmosDBStorage(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                StarWarsCosmosDBClient client = serviceScope.ServiceProvider.GetService<StarWarsCosmosDBClient>();
                client.EnsureDatabaseCreated();
                client.EnsureDatabaseSeeded();
            }

            return app;
        }
    }
}
