using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public class ManagementCompaniesUpdatedEvent : ManagementCompaniesEvent, IRequest
    {
        public ManagementCompaniesUpdatedEvent(List<ManagementCompany> updatedCompanies) : base(updatedCompanies)
        {
        }
    }
}
