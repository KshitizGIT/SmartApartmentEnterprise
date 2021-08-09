using Nest;
using Property.API.Indexing;
using Property.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Property.API.Search.ElasticSearch
{
    public class MainSearchIndexer : ISearchIndexer
    {
        private readonly IElasticClient _client;

        public MainSearchIndexer(IElasticClient client)
        {
            _client = client;
        }
        public string IndexName => "smart-apartment-search-index";

        public async Task BuildIndexAsync()
        {
            var response = await _client.Indices.ExistsAsync(IndexName);
            if (!response.Exists)
            {
                var createIndexResponse = await _client.Indices.CreateAsync(IndexName, index =>
               index.Settings(s => s
                        .Analysis(a => a.Analyzers(aa => aa.Standard("default", sa => sa.StopWords("_english_")) // Changing default index to include stopwords
                                       .Custom("marketScopeAnalyzer", m => m.Tokenizer("keyword").Filters("lowercase"))))
               ).Map<SearchResult>(map => map.AutoMap().
                                        Properties(df => df.Text(ne => ne.Name(i => i.Market).Analyzer("marketScopeAnalyzer")))
                     ));
                await Task.CompletedTask;
            }
        }

        public async Task DropDocuments(string type)
        {
            await _client.DeleteByQueryAsync<SearchResult>(c => c
            .Index(IndexName).Query(q => q.Match(m => m.Field(f => f.Type).Query(type)))
            );
        }

        public async Task IndexManyAsync(IEnumerable<SearchResult> objects)
        {
            await _client.IndexManyAsync(objects, IndexName);
        }
    }
}
