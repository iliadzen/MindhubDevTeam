using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class TrackerRepository : IRepository<Tracker>
    {
        private readonly CommonDbContext _context;

        public TrackerRepository(CommonDbContext context)
        {
            _context = context;
        }
        public void Save(Tracker entity)
        {
            _context.Trackers.Add(entity);
            _context.SaveChanges();
        }

        public Option<Tracker> Get(Guid id)
        {
            return Option<Tracker>.Some(_context.Trackers.SingleOrDefault(_ => _.Id == id));
        }

        public IReadOnlyCollection<Tracker> GetAll()
        {
            return !_context.Trackers.Any() ? new List<Tracker>() : _context.Trackers.ToList();
        }

        public void Update(Tracker entity)
        {
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            entity.Do(_ => _context.Remove(_));
        }
    }
}