using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PropertyManagement.API.BackgroundTasks
{
    public interface IPropertyImportBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(ImportTask workItem);

        ValueTask<ImportTask> DequeueAsync(
            CancellationToken cancellationToken);
    }

    public class BackgroundTaskQueue : IPropertyImportBackgroundTaskQueue
    {
        private readonly Channel<ImportTask> _queue;

        public BackgroundTaskQueue(int capacity)
        {
            // Capacity should be set based on the expected application load and
            // number of concurrent threads accessing the queue.            
            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
            // which completes only when space became available. This leads to backpressure,
            // in case too many publishers/calls start accumulating.
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<ImportTask>(options);
        }
        public async ValueTask QueueBackgroundWorkItemAsync(ImportTask workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<ImportTask> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}
