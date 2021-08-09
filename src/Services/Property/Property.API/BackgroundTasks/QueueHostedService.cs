using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Property.API.Indexing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Property.API.BackgroundTasks
{
    public class QueueHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<QueueHostedService> _logger;

        public QueueHostedService(IServiceProvider serviceProvider,
            IPropertyImportBackgroundTaskQueue taskQueue,
            ILogger<QueueHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            TaskQueue = taskQueue;
            _logger = logger;
        }

        public IPropertyImportBackgroundTaskQueue TaskQueue { get; }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                       $"Queued Hosted Service is running.{Environment.NewLine}" +
                       $"{Environment.NewLine}Tap W to add a work item to the " +
                       $"background queue.{Environment.NewLine}");

            await BackgroundProcessing(stoppingToken);
        }
        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem =
                    await TaskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var indexer = scope.ServiceProvider.GetRequiredService<ISearchIndexer>();
                    await workItem.RunAsync(indexer);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}