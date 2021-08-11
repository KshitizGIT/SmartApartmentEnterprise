using MediatR;
using PropertyManagement.API.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
{
    public class PropertiesCreatedEvent : PropertiesEvent, IRequest
    {
        public PropertiesCreatedEvent(List<Property> properties) : base(properties)
        {
        }
    }
}
