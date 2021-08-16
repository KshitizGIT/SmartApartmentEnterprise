using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManagement.API.Data;

namespace PropertyManagement.API.BackgroundTasks
{
    public class ImportTaskContext
    {
        public SmartApartmentDbContext DbContext { get; set; }
        public IMediator Mediator { get; set; }
        public ILogger Logger { get; set; }
    }
}