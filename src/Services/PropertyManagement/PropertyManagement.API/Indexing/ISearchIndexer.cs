using PropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyManagement.API.Indexing
{
    public interface ISearchIndexer
    {
        public string IndexName { get; }
        public Task BuildIndexAsync();
        public Task IndexManyAsync(IEnumerable<SearchResult> objects);
        public Task DropDocuments(string type);
    }
}
