using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveQ.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LiveQ.Api.Helpers;

namespace LiveQ.Api.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEvent(Event _event, AppUser creator)
        {
            _event.CreationDate = DateTime.Today;
            _event.Creator = creator;
            _event.CreatorId = creator.Id;
            try
            {
                await _context.Events.AddAsync(_event);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateEvent(Event _event, AppUser creator)
        {
            try
            {
                _context.Entry<Event>(_event).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<Event> GetEvents(string start, string end)
        {
            DateTime _start = DateConverter.ToDateTime(start);
            DateTime _end = DateConverter.ToDateTime(end);
            var result = new List<Event>();
            try
            {
                result = _context.Events
                .Where(s => (s.StartDate >= _start & s.EndDate <= _end))
                .Include(a => a.Subscribers)
                .Include(u => u.Creator)
                .ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public IEnumerable<Event> GetEvents(string start, string end, string creatorId)
        {
            DateTime _start = DateConverter.ToDateTime(start);
            DateTime _end = DateConverter.ToDateTime(end);
            var result = new List<Event>();
            try
            {
                result = _context.Events
                .Where(s => ((s.StartDate >= _start) && (s.EndDate <= _end) && (s.CreatorId == creatorId)))
                .Include(a => a.Subscribers)
                .ThenInclude(a => a.Subscriber)
                .Include(u => u.Creator)
                .ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public IEnumerable<Event> GetEvents()
        {
            return _context.Events
                .Include(a => a.Subscribers)
                .Include(u => u.Creator);
        }

        public Event GetEvent(int id)
        {
            return _context.Events
                .Include(a => a.Creator)
                .Include(c => c.Subscribers).FirstOrDefault(a => a.Id == id);
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            try
            {
                var enent = await _context.Events.FindAsync(eventId);
                _context.Events.Remove(enent);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsSubscriber(Event _event, AppUser user)
        {
            EventSubscriber subscription = _event.Subscribers.FirstOrDefault(a => a.Subscriber.Id == user.Id);
            if (subscription != null)
            {
                return true;
            }
            return false;
        }

        public bool IsPastDue(Event _event)
        {
            if (_event.StartDate < DateTime.Now)
            {
                return true;
            }
            return false;
        }
        

        
    }
}
