using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class EventRepository : IRepository<Event>
    {
        private readonly CommonDbContext _context;

        public EventRepository(CommonDbContext context)
        {
            _context = context;
        }
        public void Save(Event entity)
        {
            _context.Events.Add(entity);
            _context.SaveChanges();
        }

        public Option<Event> Get(Guid id)
        {
            return Option<Event>.Some(_context.Events.SingleOrDefault(_ => _.Id == id));
        }

        public IReadOnlyCollection<Event> GetAll()
        {
            return !_context.Events.Any() ? new List<Event>() : _context.Events.ToList();
        }

        public void Update(Event entity)
        {
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            entity.Do(_ => _context.Remove((object) _));
        }
    }
}