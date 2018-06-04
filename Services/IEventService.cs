using LiveQ.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetEvents(string start, string end);
        IEnumerable<Event> GetEvents(string start, string end, string creatorId);
        IEnumerable<Event> GetEvents();
        Event GetEvent(int id);
        Task<bool> CreateEvent(Event _event, AppUser creator);
        Task<bool> UpdateEvent(Event _event, AppUser creator);
        Task<bool> DeleteEventAsync(int eventtId);
        bool IsSubscriber(Event _event, AppUser user);
        bool IsPastDue(Event _event);


    }
}
