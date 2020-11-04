using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure
{
    public class CommonDbRepository<T> : IRepository<T>
    where T : class, IEntity
    {
        public CommonDbRepository(DbSet<T> set, CommonDbContext context)
        {
            _context = context;
            _set = set;
        }
        
        public virtual void Save(T entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
        }

        public virtual Option<T> Get(Guid id)
        {
            return Option<T>.Some(_set.SingleOrDefault(entity => entity.Id == id));
        }

        public virtual IReadOnlyCollection<T> GetAll()
        {
            return !_set.Any() ? new List<T>() : _set.ToList();
        }

        public virtual void Update(T entity)
        {
            _context.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            var optionalEntity = Get(id);
            optionalEntity.Do(entity =>
            {
                _set.Remove(entity);
            });
        }
        
        private readonly CommonDbContext _context;
        private readonly DbSet<T> _set;
    }
}