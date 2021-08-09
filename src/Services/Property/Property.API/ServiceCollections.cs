using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Property.API.BackgroundTasks;
using Property.API.Indexing;
using Property.API.Providers.ElasticSearch;
using Property.API.Search;
using Property.API.Search.ElasticSearch;

namespace Property.API
{
    public static class ServiceCollections
    {
        public static IServiceCollection AddSmartApartmentServices(this IServiceCollection collection, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new System.Uri("http://localhost:9200")).EnableDebugMode();

            collection.AddScoped<IElasticClient>(s => new ElasticClient(settings));
            collection.AddScoped<ISearchIndexer, MainSearchIndexer>();
            collection.AddScoped<ISearchProvider, ElasticSearchProvider>();
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
