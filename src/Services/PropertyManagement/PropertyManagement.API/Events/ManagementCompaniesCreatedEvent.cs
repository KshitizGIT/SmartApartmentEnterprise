using MediatR;
using PropertyManagement.API.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
{
    public class ManagementCompaniesCreatedEvent : ManagementCompaniesEvent, IRequest
    {
        public ManagementCompaniesCreatedEvent(List<ManagementCompany> companies) : base(companies)
        {
        }

    }
}
