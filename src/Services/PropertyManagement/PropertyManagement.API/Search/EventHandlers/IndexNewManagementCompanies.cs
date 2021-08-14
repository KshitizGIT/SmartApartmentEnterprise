using MediatR;
using Nest;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.EventHandlers
{
    public class IndexNewManagementCompanies : AsyncRequestHandler<ManagementCompaniesCreatedEvent>
    {
        private readonly IElasticClient _client;

        public IndexNewManagementCompanies(IElasticClient client)
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
