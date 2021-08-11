using PropertyManagement.API.Indexing;
using PropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyManagement.API.Search
{
    public interface ISearchProvider
    {
        public ISearchIndexer Indexer { get; }
        public Task<IEnumerable<SearchResult>> SearchEntities(string searchString, string market, int limit);
    }
}
