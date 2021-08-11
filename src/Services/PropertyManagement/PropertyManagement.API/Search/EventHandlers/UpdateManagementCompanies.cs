using MediatR;
using Nest;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using PropertyManagement.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.Search.EventHandlers
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
