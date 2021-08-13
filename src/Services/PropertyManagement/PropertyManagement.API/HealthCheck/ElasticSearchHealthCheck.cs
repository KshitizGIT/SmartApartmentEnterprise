using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.HealthCheck
{
    public class ElasticSearchHealthCheck : IHealthCheck
    {
        public ElasticSearchHealthCheck()
        {
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var settings = new ConnectionSettings(new System.Uri("http://elasticsearch:9200"));
                var client = new ElasticClient(settings);
                var pingResult = await client.PingAsync(ct: cancellationToken);
                var isSuccess = pingResult.ApiCall.HttpStatusCode == 200;
                return isSuccess ? HealthCheckResult.Healthy() : new HealthCheckResult(context.Registration.FailureStatus);
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}
