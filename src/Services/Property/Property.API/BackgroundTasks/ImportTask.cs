using Property.API.Indexing;
using System;
using System.Threading.Tasks;

namespace Property.API.BackgroundTasks
{
    public abstract class ImportTask
    {
        public ImportTask(string filePath) : this(Guid.NewGuid(), filePath)
        {
        }
        public ImportTask(Guid id, string filePath)
        {
            Id = id;
            FilePath = filePath;
        }

        public Guid Id { get; }
        public string FilePath { get; }
        public string Status { get; }
        public abstract Task RunAsync(ISearchIndexer indexer);
    }
}
