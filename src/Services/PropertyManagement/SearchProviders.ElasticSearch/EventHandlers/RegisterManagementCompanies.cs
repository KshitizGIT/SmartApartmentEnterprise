using MediatR;
using Nest;
using PropertyManagement.Infrastructure.Events;
using PropertyManagement.Infrastructure.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.SearchProviders.ElasticSearch.EventHandlers
{
    public class RegisterManagementCompanies : AsyncRequestHandler<ManagementCompaniesCreatedEvent>
    {
        private readonly IElasticClient _client;

        public RegisterManagementCompanies(IElasticClient client)
        {
            _client = client;
        }

        protected async override Task Handle(ManagementCompaniesCreatedEvent request, CancellationToken cancellationToken)
        {
            var searchObjects = request.Companies.Select(s => s.ToSearchResult());
            await _client.IndexManyAsync(searchObjects);
        }
    }
}
