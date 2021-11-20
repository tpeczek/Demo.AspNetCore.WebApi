using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Demo.AspNetCore.WebApi.Http;
using Demo.AspNetCore.WebApi.Http.ConditionalRequests;
using Demo.AspNetCore.WebApi.Services;
using Demo.AspNetCore.WebApi.Services.Cosmos;

namespace Demo.AspNetCore.WebApi
{
    public class Program
    {
        private const string COSMOS_CONFIGURATION_SECTION = "Cosmos";

        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            WebApplication app = builder.Build();

            await ConfigureWebApplication(app);

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.Configure<CosmosOptions>(builder.Configuration.GetSection(COSMOS_CONFIGURATION_SECTION));
            builder.Services.AddSingleton<IStarWarsCosmosClient, StarWarsCosmosClient>();

            builder.Services.AddMediatR(typeof(Program));

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ConditionalRequestsFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        private static async Task ConfigureWebApplication(WebApplication app)
        {
            await app.InitializeStarWarsCosmosAsync();

            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();
        }
    }
}
