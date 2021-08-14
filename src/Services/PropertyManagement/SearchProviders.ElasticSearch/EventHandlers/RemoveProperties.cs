using MediatR;
using Nest;
using PropertyManagement.Infrastructure.Events;
using PropertyManagement.Infrastructure.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.SearchProviders.ElasticSearch.EventHandlers
{
    public class RemoveProperties : AsyncRequestHandler<PropertiesDeletedEvent>
    {
        private readonly IElasticClient _client;

        public RemoveProperties(IElasticClient elasticClient)
        {
            _client = elasticClient;
        }


        protected async override Task Handle(PropertiesDeletedEvent request, CancellationToken cancellationToken)
        {
            var objects = request.Properties.Select(c => c.ToSearchResult());
            await _client.DeleteManyAsync(objects);
        }
    }
}
