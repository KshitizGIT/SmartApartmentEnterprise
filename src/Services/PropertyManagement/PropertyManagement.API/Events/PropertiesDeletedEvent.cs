using MediatR;
using PropertyManagement.API.Models;
using System.Collections.Generic;

namespace PropertyManagement.API.Events
{
    public class PropertiesDeletedEvent : PropertiesEvent, IRequest
    {
        public PropertiesDeletedEvent(List<Property> properties) :base(properties)
        {
        }

    }
}
