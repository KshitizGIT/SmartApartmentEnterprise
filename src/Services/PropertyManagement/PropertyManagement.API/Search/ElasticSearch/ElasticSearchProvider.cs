using Nest;
using PropertyManagement.API.Search;
using PropertyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagement.API.Providers.ElasticSearch
{
    public class ElasticSearchService : ISearchService
    {
        private readonly IElasticClient _elasticClient;
        public ElasticSearchService(IElasticClient client)
        {
            _elasticClient = client;
        }


        public async Task<IEnumerable<SearchResult>> SearchEntities(string searchString, string market, int limit)
        {
            var result = await _elasticClient.SearchAsync<SearchResult>(s => s.Size(limit).
                                   Query(q => q.Bool(b => b.Must(m => m.SimpleQueryString(q => q.Query(searchString)))
                                             .Filter(f => f.Match(m => m.Field(fe => fe.Market).Query(market)
                                     )))));
            return result.Hits.Select(e => e.Source);
        }
    }
}
