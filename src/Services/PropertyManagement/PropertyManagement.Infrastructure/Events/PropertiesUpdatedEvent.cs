using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public class PropertiesUpdatedEvent : PropertiesEvent, IRequest
    {
        public PropertiesUpdatedEvent(List<Property> properties) : base(properties)
        {
        }

    }
}
