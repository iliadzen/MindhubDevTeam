using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class LicenseRepository : IRepository<License>
    {
        public LicenseRepository(CommonDbContext context)
        {
            _context = context;
        }

        public void Save(License entity)
        {
            _context.Licenses.Add(entity);
            _context.SaveChanges();
        }

        public Option<License> Get(Guid id)
        {
            return Option<License>.Some(_context.Licenses.SingleOrDefault(_ => _.Id == id));
        }

        public IReadOnlyCollection<License> GetAll()
        {
            return !_context.Licenses.Any() ? new List<License>() : _context.Licenses.ToList();
        }

        public void Update(License entity)
        {
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            entity.Do(_ => _context.Remove((object) _));
        }
        
        private readonly CommonDbContext _context;
    }
}