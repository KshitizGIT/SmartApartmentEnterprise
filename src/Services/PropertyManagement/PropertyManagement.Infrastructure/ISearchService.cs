using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyManagement.Infrastructure
{
    public interface ISearchService
    {
        public Task<IEnumerable<SearchResult>> SearchEntities(string searchString, string market, int limit);
    }
}
