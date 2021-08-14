using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public class ManagementCompaniesDeletedEvent : ManagementCompaniesEvent, IRequest
    {
        public ManagementCompaniesDeletedEvent(List<ManagementCompany> managementCompanies) : base(managementCompanies)
        {
        }

    }
}
