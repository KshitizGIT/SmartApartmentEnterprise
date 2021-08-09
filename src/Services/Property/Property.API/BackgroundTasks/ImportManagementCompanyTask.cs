using Property.API.DTOs;
using Property.API.Extensions;
using Property.API.Indexing;
using Property.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Property.API.BackgroundTasks
{
    public class ImportManagementCompanyTask : ImportTask
    {
        public ImportManagementCompanyTask(string filePath) : base(filePath)
        {

        }
        public async override Task RunAsync(ISearchIndexer indexer)
        {

            using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var mgmtCompanies = await JsonSerializer.DeserializeAsync<List<ManagementCompanyResultDTO>>(stream);

            var records = mgmtCompanies.Select(m => m.ToSearchResult()).ToList();
            await indexer.DropDocuments(nameof(ManagementCompany));
            await indexer.IndexManyAsync(records);
        }
    }
}
