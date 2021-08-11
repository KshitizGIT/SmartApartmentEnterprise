using MediatR;
using PropertyManagement.API.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
{
    public class ManagementCompaniesDeletedEvent : ManagementCompaniesEvent, IRequest
    {
        public ManagementCompaniesDeletedEvent(List<ManagementCompany> managementCompanies) : base(managementCompanies)
        {
        }

    }
}
