using MediatR;
using Nest;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.API.EventHandlers
{
    public class RemoveManagementCompanies : AsyncRequestHandler<ManagementCompaniesDeletedEvent>
    {
        private readonly IElasticClient _client;

        public RemoveManagementCompanies(IElasticClient elasticClient)
        {
            _client = elasticClient;
        }

        protected async override Task Handle(ManagementCompaniesDeletedEvent request, CancellationToken cancellationToken)
        {
            var objects = request.Companies.Select(c => c.ToSearchResult());
            await _client.DeleteManyAsync(objects);
        }
    }
}
