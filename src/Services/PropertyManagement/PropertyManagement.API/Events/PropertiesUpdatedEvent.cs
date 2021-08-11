using MediatR;
using PropertyManagement.API.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
{
    public class PropertiesUpdatedEvent : PropertiesEvent, IRequest
    {
        public PropertiesUpdatedEvent(List<Property> properties) : base(properties)
        {
        }

    }
}
