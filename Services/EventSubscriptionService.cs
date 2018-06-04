using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveQ.Api.Models;
using Microsoft.EntityFrameworkCore;
using LiveQ.Api.Helpers;

namespace LiveQ.Api.Services
{
    public class EventSubscriptionService : IEventSubscriptionService
    {
        private readonly ApplicationDbContext _context;

        public EventSubscriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateSubscriptionAsync(EventSubscriber subscription, AppUser subscriber)
        {
            subscription.Subscriber = subscriber;
            subscription.SubscriberId = subscriber.Id;
            try
            {
                await _context.EventSubscribers.AddAsync(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteSubscriptionAsync(EventSubscriber es)
        {
            try
            {
                _context.EventSubscribers.Remove(es);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public EventSubscriber GetSubscription(int id)
        {
            return _context.EventSubscribers
                .Include(a => a.Event.Creator)
                .Include(a => a.Subscriber)
                .Include(c => c.Event).FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<EventSubscriber> GetSubscriptions(string start, string end)
        {
            DateTime _start = DateConverter.ToDateTime(start);
            DateTime _end = DateConverter.ToDateTime(end);
            var result = new List<EventSubscriber>();
            try
            {
                result = _context.EventSubscribers
                .Where(s => (s.Start >= _start & s.End <= _end))
                .Include(a => a.Event)
                .Include(a => a.Event.Creator)
                .Include(u => u.Subscriber)
                .ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public IEnumerable<EventSubscriber> GetSubscriptions(string start, string end, string subscriberId)
        {
            DateTime _start = DateConverter.ToDateTime(start);
            DateTime _end = DateConverter.ToDateTime(end);
            var result = new List<EventSubscriber>();
            try
            {
                result = _context.EventSubscribers
                .Where(s => ((s.Start >= _start) && (s.End <= _end) && (s.SubscriberId == subscriberId)))
                .Include(a => a.Event)
                .Include(u => u.Event.Creator)
                .Include(u => u.Subscriber)
                .ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public IEnumerable<EventSubscriber> GetSubscriptions()
        {
            return _context.EventSubscribers
                .Include(a => a.Subscriber)
                .Include(u => u.Event);
        }

        public async Task<bool> UpdateSubscriptionAsync(EventSubscriber subscription, AppUser creator)
        {
            try
            {
                _context.Entry<EventSubscriber>(subscription).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public EventSubscriber InitializeSubscription(Event _event, AppUser subscriber)
        {
            int minutes = (int)(_event.TimeLimit.TotalMinutes * (_event.Subscribers.Count));
            int span = (int)(_event.TimeLimit.TotalMinutes * (_event.Subscribers.Count + 1));
            EventSubscriber _subscriber = new EventSubscriber
            {
                SubscriberId = subscriber.Id /*User.Identity.GetUserId()*/,
                JoinDate = DateTime.Now,
                Event = _event,
                EventId = _event.Id,
                Start = _event.StartDate.AddMinutes(minutes),
                End = _event.StartDate.AddMinutes(span),
                Subscriber = subscriber
            };

            return _subscriber;

        }
    }
}
