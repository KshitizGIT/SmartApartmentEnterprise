using MediatR;
using Nest;
using PropertyManagement.Infrastructure;
using PropertyManagement.Infrastructure.Events;
using PropertyManagement.Infrastructure.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.SearchProviders.ElasticSearch.EventHandlers
{
    public class UpdateManagementCompanies : AsyncRequestHandler<ManagementCompaniesUpdatedEvent>
    {
        private readonly IElasticClient _elasticClient;

        public UpdateManagementCompanies(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        protected async override Task Handle(ManagementCompaniesUpdatedEvent request, CancellationToken cancellationToken)
        {
            var records = request.Companies.Select(s => s.ToSearchResult());
            foreach (var entry in records)
            {
                await _elasticClient.UpdateAsync<SearchResult>(entry.Id, e => e.Doc(entry));
            }

        }
    }
}
