using MediatR;
using Nest;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.EventHandlers
{
    public class IndexNewProperties : AsyncRequestHandler<PropertiesCreatedEvent>
    {
        private readonly IElasticClient _client;

        public IndexNewProperties(IElasticClient client)
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
