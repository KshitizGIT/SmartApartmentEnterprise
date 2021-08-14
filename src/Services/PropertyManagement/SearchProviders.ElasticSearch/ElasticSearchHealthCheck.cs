using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.SearchProviders.ElasticSearch
{
    public class ElasticSearchHealthCheck : IHealthCheck
    {

        private readonly IElasticClient _client;

        public ElasticSearchHealthCheck(IElasticClient client)
        {
            _client = client;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var pingResult = await _client.PingAsync(ct: cancellationToken);
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
