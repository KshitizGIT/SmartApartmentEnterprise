using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PropertyManagement.API.BackgroundTasks;
using PropertyManagement.API.Options;
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

    }
}
