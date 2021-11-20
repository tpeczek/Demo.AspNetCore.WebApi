using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Demo.AspNetCore.WebApi.Services.Cosmos
{
    internal interface IStarWarsCosmosClient
    {
        Container Characters { get; }

        Task EnsureDatabaseAndContainerExistsAsync();

        Task EnsureDatabaseSeededAsync();
    }
}
