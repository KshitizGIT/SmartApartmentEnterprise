using MediatR;
using Nest;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using PropertyManagement.API.Indexing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.EventHandlers
{
    public class RemoveProperties : AsyncRequestHandler<PropertiesDeletedEvent>
    {
        private readonly IElasticClient _client;
        private readonly ISearchIndexer _indexer;

        public RemoveProperties(IElasticClient elasticClient, ISearchIndexer indexer)
        {
            _client = elasticClient;
            _indexer = indexer;
        }


        protected async override Task Handle(PropertiesDeletedEvent request, CancellationToken cancellationToken)
        {
            var objects = request.Properties.Select(c => c.ToSearchResult());
            await _client.DeleteManyAsync(objects, _indexer.IndexName);
        }
    }
}
