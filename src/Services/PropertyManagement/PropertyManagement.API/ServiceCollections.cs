using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using PropertyManagement.API.BackgroundTasks;
using PropertyManagement.API.Data;
using PropertyManagement.API.Indexing;
using PropertyManagement.API.Providers.ElasticSearch;
using PropertyManagement.API.Search;
using PropertyManagement.API.Search.ElasticSearch;

namespace PropertyManagement.API
{
    public static class ServiceCollections
    {
        public static IServiceCollection AddSmartApartmentServices(this IServiceCollection collection, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new System.Uri($"{configuration["ELASTICSEARCH_URL"]}")).
                EnableDebugMode().
                DefaultIndex("smart-apartment-search-index");

            collection.AddScoped<IElasticClient>(s => new ElasticClient(settings));
            collection.AddScoped<ISearchIndexer, MainSearchIndexer>();
            collection.AddScoped<ISearchProvider, ElasticSearchProvider>();
            collection.AddDbContext<SmartApartmentDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
                );
            collection.AddHostedService<QueueHostedService>();
            collection.AddSingleton<IPropertyImportBackgroundTaskQueue>(ctx =>
            {
                if (!int.TryParse(configuration["QueueCapacity"], out var queueCapacity))
                    queueCapacity = 2;
                return new BackgroundTaskQueue(queueCapacity);
            });
            return collection;

        }
    }
}
