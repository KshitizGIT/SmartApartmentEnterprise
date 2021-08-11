using MediatR;
using PropertyManagement.API.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
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
