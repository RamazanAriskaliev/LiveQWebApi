using LiveQ.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.Services
{
    public interface IEventSubscriptionService
    {
        IEnumerable<EventSubscriber> GetSubscriptions(string start, string end);
        IEnumerable<EventSubscriber> GetSubscriptions(string start, string end, string subscriberId);
        IEnumerable<EventSubscriber> GetSubscriptions();
        EventSubscriber GetSubscription(int id);
        Task<bool> CreateSubscriptionAsync(EventSubscriber subscription, AppUser creator);
        Task<bool> UpdateSubscriptionAsync(EventSubscriber subscription, AppUser creator);
        bool DeleteSubscriptionAsync(EventSubscriber es);

        EventSubscriber InitializeSubscription(Event _event, AppUser subscriber);
    }
}
