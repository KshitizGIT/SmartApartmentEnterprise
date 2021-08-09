using Nest;
using Property.API.Indexing;
using Property.API.Search;
using Property.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Property.API.Providers.ElasticSearch
{
    public class ElasticSearchProvider : ISearchProvider
    {
        private readonly IElasticClient _elasticClient;
        public ElasticSearchProvider(IElasticClient client, ISearchIndexer indexer)
        {
            _elasticClient = client;
            Indexer = indexer;
        }

        public ISearchIndexer Indexer { get; private set; }

        public async Task<IEnumerable<SearchResult>> SearchEntities(string searchString, string market, int limit)
        {
            var result = await _elasticClient.SearchAsync<SearchResult>(s => s.Index(Indexer.IndexName).Size(limit).
                                   Query(q => q.Bool(b => b.Must(m => m.SimpleQueryString(q => q.Query(searchString)))
                                             .Filter(f => f.Match(m => m.Field(fe => fe.Market).Query(market)
                                     )))));
            return result.Hits.Select(e => e.Source);
        }
    }
}
