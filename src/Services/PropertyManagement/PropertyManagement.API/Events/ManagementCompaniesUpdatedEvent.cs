using MediatR;
using PropertyManagement.API.Models;
using PropertyManagement.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
{
    public class ManagementCompaniesUpdatedEvent : ManagementCompaniesEvent, IRequest
    {
        public ManagementCompaniesUpdatedEvent(List<ManagementCompany> updatedCompanies) : base(updatedCompanies)
        {
        }
    }
}
