using MediatR;
using PropertyManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace PropertyManagement.Infrastructure.Events
{
    public class PropertiesDeletedEvent : PropertiesEvent, IRequest
    {
        public PropertiesDeletedEvent(List<Property> properties) : base(properties)
        {
        }

    }
}
