using AutoMapper;
using LiveQ.Api.Helpers;
using LiveQ.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels.Mappinngs
{
    public class EventSubscriberDTOConverter : ITypeConverter<EventSubscriber, EventSubscriberDTO>
    {
        public EventSubscriberDTO Convert(EventSubscriber source, EventSubscriberDTO destination, ResolutionContext context)
        {
            destination = new EventSubscriberDTO();
            destination.id = source.Id;
            destination.start = DateConverter.ToUnixTime(source.Start);
            destination.end = DateConverter.ToUnixTime(source.End);
            destination.targetEvent = new EventDTO()
            {
                id = source.Event.Id,
                end = DateConverter.ToUnixTime(source.Event.EndDate),
                start = DateConverter.ToUnixTime(source.Event.StartDate),
                timeLimit = source.Event.TimeLimit.ToString(),
                title = source.Event.Title,
                creator = new UserDTO()
                {
                    userName = source.Event.Creator.UserName,
                    firstName = source.Event.Creator.FirstName,
                    lastName = source.Event.Creator.LastName,
                    id = source.Event.Creator.Id
                }
            };
            return destination;
        }
    }
}
