using MediatR;
using PropertyManagement.API.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
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
