using MediatR;
using Nest;
using PropertyManagement.Infrastructure.Events;
using PropertyManagement.Infrastructure.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.SearchProviders.ElasticSearch.EventHandlers
{
    public class RemoveManagementCompanies : AsyncRequestHandler<ManagementCompaniesDeletedEvent>
    {
        private readonly IElasticClient _client;

        public RemoveManagementCompanies(IElasticClient elasticClient)
        {
            _client = elasticClient;
        }

        protected async override Task Handle(ManagementCompaniesDeletedEvent request, CancellationToken cancellationToken)
        {
            var objects = request.Companies.Select(c => c.ToSearchResult());
            await _client.DeleteManyAsync(objects);
        }
    }
}
