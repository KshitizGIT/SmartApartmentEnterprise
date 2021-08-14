using Nest;
using PropertyManagement.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagement.SearchProviders.ElasticSearch
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
                                   Query(q => q
                                     .Bool(b => b
                                       .Must(m => m
                                         .SimpleQueryString(q => q
                                           .Query(searchString)
                                           .Fields(s => s
                                             .Field(f => f.Name)
                                             .Field(f => f.FormerName)
                                             .Field(f => f.City)
                                             .Field(f => f.State)
                                             .Field(f => f.StreetAddress)
                                             )
                                           )
                                       )
                                       .Filter(f => f.Match(m => m.Field(fe => fe.Market).Query(market)
                                     )))));
            return result.Hits.Select(e => e.Source);
        }
    }
}
