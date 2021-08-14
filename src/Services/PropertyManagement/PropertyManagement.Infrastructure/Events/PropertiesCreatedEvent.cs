using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public class PropertiesCreatedEvent : PropertiesEvent, IRequest
    {
        public PropertiesCreatedEvent(List<Property> properties) : base(properties)
        {
        }
    }
}
