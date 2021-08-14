using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public abstract class PropertiesEvent : IRequest
    {
        public PropertiesEvent(List<Property> properties)
        {
            Properties = properties;
        }

        public List<Property> Properties { get; }
    }
}
