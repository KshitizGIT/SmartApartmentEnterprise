using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public abstract class ManagementCompaniesEvent : IRequest
    {
        protected ManagementCompaniesEvent(List<ManagementCompany> companies)
        {
            Companies = companies;
        }

        public List<ManagementCompany> Companies { get; }
    }
}
