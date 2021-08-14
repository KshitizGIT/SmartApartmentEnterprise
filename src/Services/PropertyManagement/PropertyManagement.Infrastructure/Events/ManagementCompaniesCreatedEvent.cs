using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public class ManagementCompaniesCreatedEvent : ManagementCompaniesEvent, IRequest
    {
        public ManagementCompaniesCreatedEvent(List<ManagementCompany> companies) : base(companies)
        {
        }

    }
}
