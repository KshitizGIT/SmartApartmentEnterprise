using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PropertyManagement.API.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.BackgroundTasks
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
            _logger.LogInformation($"Queued Hosted Service is running.{Environment.NewLine}");

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
                    var dbContext = scope.ServiceProvider.GetRequiredService<SmartApartmentDbContext>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ImportTask>>();

                    var context = new ImportTaskContext()
                    {
                        DbContext = dbContext,
                        Mediator = mediator,
                        Logger = logger
                    };

                    await workItem.RunAsync(context);
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