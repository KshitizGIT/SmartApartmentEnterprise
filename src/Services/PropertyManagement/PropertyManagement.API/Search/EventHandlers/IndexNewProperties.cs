using MediatR;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using PropertyManagement.API.Indexing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.EventHandlers
{
    public class IndexNewProperties : AsyncRequestHandler<PropertiesCreatedEvent>
    {
        private readonly ISearchIndexer _indexer;

        public IndexNewProperties(ISearchIndexer indexer)
        {
            _indexer = indexer;
        }

        protected async override Task Handle(PropertiesCreatedEvent request, CancellationToken cancellationToken)
        {
            var searchObjects = request.Properties.Select(s => s.ToSearchResult());
            await _indexer.IndexManyAsync(searchObjects);
        }
    }
}
