using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Demo.AspNetCore.Mvc.CosmosDB.Services;
using Demo.AspNetCore.Mvc.CosmosDB.Middlewares;

namespace Demo.AspNetCore.Mvc.CosmosDB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCosmosDB(Configuration);

            services.AddMediatR(typeof(Startup).Assembly);

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCosmosDBStorage();

            app.UseMiddleware<HeadMethodMiddleware>();

            app.UseMvc();

            app.Run(async (context) =>
            {
                context.Response.ContentLength = 34;
                await context.Response.WriteAsync("-- Demo.AspNetCore.Mvc.CosmosDB --");
            });
        }
    }
}
