using MediatR;
using Nest;
using PropertyManagement.Infrastructure.Events;
using PropertyManagement.Infrastructure.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.SearchProviders.ElasticSearch.EventHandlers
{
    public class RegisterProperties : AsyncRequestHandler<PropertiesCreatedEvent>
    {
        private readonly IElasticClient _client;

        public RegisterProperties(IElasticClient client)
        {
            _client = client;
        }

        protected async override Task Handle(PropertiesCreatedEvent request, CancellationToken cancellationToken)
        {
            var searchObjects = request.Properties.Select(s => s.ToSearchResult());
            await _client.IndexManyAsync(searchObjects);
        }
    }
}
