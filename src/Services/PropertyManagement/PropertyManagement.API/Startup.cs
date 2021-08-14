using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PropertyManagement.API.Data;
using PropertyManagement.API.Extensions;
using PropertyManagement.API.HealthCheck;
using System;
using System.Text.Json.Serialization;

namespace PropertyManagement.API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(Configuration["WEBAPP_URL"])
                        .AllowAnyHeader();
                    });
            });

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["AUTH_URL"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false
                    };
                });

            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
            });

            services.AddSwagger();
            services.AddTaskRunnerHost(t =>
                        t.ThreadCount = Convert.ToInt32(Configuration["TaskRunnerThreadCount"] ?? "2")
                        );

            services.AddElasticSearch(s =>
            {
                s.EndpointUrl = Configuration["ELASTICSEARCH_URL"];
                s.SearchIndexName = "smart-apartment-search-index";
            });
            services.AddDbContext<SmartApartmentDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddMediatR(typeof(Startup));

            services.AddHealthChecks()
                .AddCheck("elastic-search-check", new ElasticSearchHealthCheck(),
                HealthStatus.Unhealthy,
                tags: new string[] { "elasticsearch" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Property.API v1");
                    c.OAuthClientId("swagger");
                    c.OAuthClientSecret("511536EF-F270-4058-80CA-1C89C192F69A");
                });
            }
        }
    }
}
