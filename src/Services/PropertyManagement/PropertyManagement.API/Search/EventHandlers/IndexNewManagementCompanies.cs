using MediatR;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using PropertyManagement.API.Indexing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.EventHandlers
{
    public class IndexNewManagementCompanies : AsyncRequestHandler<ManagementCompaniesCreatedEvent>
    {
        private readonly ISearchIndexer _indexer;

        public IndexNewManagementCompanies(ISearchIndexer indexer)
        {
            _indexer = indexer;
        }

        protected async override Task Handle(ManagementCompaniesCreatedEvent request, CancellationToken cancellationToken)
        {
            var searchObjects = request.Companies.Select(s => s.ToSearchResult());
            await _indexer.IndexManyAsync(searchObjects);
        }
    }
}
