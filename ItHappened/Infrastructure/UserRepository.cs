using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ItHappened.Infrastructure
{
    public class UserRepository : IRepository<User>
    {
        private readonly CommonDbContext _context;

        public UserRepository(CommonDbContext context)
        {
            _context = context;
        }

        public void Save(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public Option<User> Get(Guid id)
        {
            return Option<User>.Some(_context.Users.Include(_ => _.License).SingleOrDefault(_ => _.Id == id));
        }

        public IReadOnlyCollection<User> GetAll()
        {
            return !_context.Users.Any() ? new List<User>() : _context.Users.Include(_ => _.License).ToList();
        }

        public void Update(User entity)
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