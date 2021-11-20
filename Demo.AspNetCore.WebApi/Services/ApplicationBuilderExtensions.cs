using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Demo.AspNetCore.WebApi.Services.Cosmos;

namespace Demo.AspNetCore.WebApi.Services
{
    internal static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> InitializeStarWarsCosmosAsync(this IApplicationBuilder app)
        {
            IStarWarsCosmosClient starWarsCosmosClient = app.ApplicationServices.GetRequiredService<IStarWarsCosmosClient>();

            await starWarsCosmosClient.EnsureDatabaseAndContainerExistsAsync();

            await starWarsCosmosClient.EnsureDatabaseSeededAsync();

            return app;
        }
    }
}
