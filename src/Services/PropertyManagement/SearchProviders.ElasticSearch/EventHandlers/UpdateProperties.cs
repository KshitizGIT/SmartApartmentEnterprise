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
    public class UpdateProperties : AsyncRequestHandler<PropertiesUpdatedEvent>
    {
        private readonly IElasticClient _elasticClient;

        public UpdateProperties(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        protected async override Task Handle(PropertiesUpdatedEvent request, CancellationToken cancellationToken)
        {
            var records = request.Properties.Select(s => s.ToSearchResult());
            foreach (var entry in records)
            {
                await _elasticClient.UpdateAsync<SearchResult>(entry.Id, e => e.Doc(entry));
            }

        }
    }
}
