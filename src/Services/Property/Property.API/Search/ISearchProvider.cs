using Property.API.Indexing;
using Property.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Property.API.Search
{
    public interface ISearchProvider
    {
        public ISearchIndexer Indexer { get; }
        public Task<IEnumerable<SearchResult>> SearchEntities(string searchString, string market, int limit);
    }
}
