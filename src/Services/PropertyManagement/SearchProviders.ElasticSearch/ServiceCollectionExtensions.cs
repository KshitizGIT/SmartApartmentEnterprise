using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;
using PropertyManagement.Infrastructure;
using System;

namespace PropertyManagement.SearchProviders.ElasticSearch
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection collection,
            Action<ElasticSearchOptions> elasticSearchOptions)
        {
            var options = new ElasticSearchOptions();
            elasticSearchOptions?.Invoke(options);

            var settings = new ConnectionSettings(new Uri(options.EndpointUrl)).DefaultIndex(options.SearchIndexName);
            var client = new ElasticClient(settings);
            CreateSearchIndex(client, options.SearchIndexName);

            // Adding as singleton as recommended by docs.
            // See: https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/lifetimes.html
            collection.AddSingleton<IElasticClient>(client);
            collection.AddScoped<ISearchService, ElasticSearchService>();
            collection.AddHealthChecks().AddCheck("elastic-search-check", new ElasticSearchHealthCheck(client),
                HealthStatus.Unhealthy,
                tags: new string[] { "elasticsearch" });


            collection.AddMediatR(typeof(ServiceCollectionExtensions));
            return collection;
        }
        private static void CreateSearchIndex(IElasticClient client, string index)
        {
            var response = client.Indices.Exists(index);
            if (!response.Exists)
            {
                client.Indices.Create(index, index => index
                                 .Settings(s =>
                                   s.Analysis(a => a
                                      .Analyzers(aa => aa
                                        // Changing default index by adding _english stop words
                                        .Standard("default", sa => sa
                                          .StopWords("_english_"))
                                        .Custom("marketScopeAnalyzer", m => m
                                          .Tokenizer("keyword").Filters("lowercase")))))
                                 .Map<SearchResult>(map => map
                                   .AutoMap().Properties(df => df
                                     .Text(ne => ne.Name(i => i.Market).Analyzer("marketScopeAnalyzer")))
                                   ));

            }
        }
    }
}

