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
    public class RemoveManagementCompanies : AsyncRequestHandler<ManagementCompaniesDeletedEvent>
    {
        private readonly IElasticClient _client;
        private readonly ISearchIndexer _indexer;

        public RemoveManagementCompanies(IElasticClient elasticClient, ISearchIndexer indexer)
        {
            _client = elasticClient;
            _indexer = indexer;
        }

        protected async override Task Handle(ManagementCompaniesDeletedEvent request, CancellationToken cancellationToken)
        {
            var objects = request.Companies.Select(c => c.ToSearchResult());
            await _client.DeleteManyAsync(objects, _indexer.IndexName);
        }
    }
}
