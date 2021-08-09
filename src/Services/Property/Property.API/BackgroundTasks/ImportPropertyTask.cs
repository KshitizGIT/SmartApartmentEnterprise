using Property.API.DTOs;
using Property.API.Extensions;
using Property.API.Indexing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Property.API.BackgroundTasks
{
    public class ImportPropertyTask : ImportTask
    {
        public ImportPropertyTask(string filePath) : base(filePath)
        {

        }
        public ImportPropertyTask(Guid id, string filePath) : base(id, filePath)
        {
        }

        public async override Task RunAsync(ISearchIndexer indexer)
        {
            try
            {
                using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                var properties = await JsonSerializer.DeserializeAsync<List<PropertyResultDTO>>(stream);
                var records = properties.Select(p => p.ToSearchResult()).ToList();

                await indexer.DropDocuments(nameof(Property));
                await indexer.IndexManyAsync(records);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
