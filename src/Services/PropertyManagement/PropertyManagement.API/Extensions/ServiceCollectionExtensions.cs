using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nest;
using PropertyManagement.API.BackgroundTasks;
using PropertyManagement.API.Options;
using PropertyManagement.API.Providers.ElasticSearch;
using PropertyManagement.API.Search;
using PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PropertyManagement.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Property.API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        ClientCredentials = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri("http://localhost:500/connect/authorize"),
                            TokenUrl = new Uri("http://localhost:5000/connect/token"),
                            Scopes = new Dictionary<string, string> { { "Property.API", "Access Property APIs" }, }
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } }, new[] { "Property.API" } } });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            return services;
        }
        public static IServiceCollection AddTaskRunnerHost(this IServiceCollection services, Action<TaskRunnerOptions> options)
        {
            services.AddHostedService<QueueHostedService>();
            var taskRunnerOptions = new TaskRunnerOptions();
            services.AddSingleton<IPropertyImportBackgroundTaskQueue>(ctx =>
            {
                options?.Invoke(taskRunnerOptions);
                return new BackgroundTaskQueue(taskRunnerOptions.ThreadCount);
            });
            return services;
        }
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
