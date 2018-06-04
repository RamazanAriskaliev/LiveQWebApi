using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LiveQ.Api.Models;
using LiveQ.Api.Helpers;

namespace LiveQ.Api.ViewModels.Mappinngs
{
    public class EventDTOConverter : ITypeConverter<Event, EventDTO>
    {
        public EventDTO Convert(Event source, EventDTO destination, ResolutionContext context)
        {
            destination = new EventDTO();
            destination.id = source.Id;
            destination.title = source.Title;
            destination.start = DateConverter.ToUnixTime(source.StartDate);
            destination.end = DateConverter.ToUnixTime(source.EndDate);
            destination.timeLimit = source.TimeLimit.ToString();
            foreach (var item in source.Subscribers)
            {
                destination.subscribers.Add(new EventSubscriberDTO
                {
                    id = item.Id,
                    subscriber = new UserDTO
                    {
                        id = item.Subscriber.Id,
                        userName = item.Subscriber.UserName,
                        firstName = item.Subscriber.FirstName,
                        lastName = item.Subscriber.LastName
                    },

                    start = DateConverter.ToUnixTime(source.StartDate),
                    end = DateConverter.ToUnixTime(source.EndDate)
                });
            }
            return destination;
        }
    }
}
