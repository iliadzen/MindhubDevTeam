using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly CommonDbContext _context;

        public CommentRepository(CommonDbContext context)
        {
            _context = context;
        }
        public void Save(Comment entity)
        {
            _context.Comments.Add(entity);
            _context.SaveChanges();
        }

        public Option<Comment> Get(Guid id)
        {
            return Option<Comment>.Some(_context.Comments.SingleOrDefault(_ => _.Id == id));
        }

        public IReadOnlyCollection<Comment> GetAll()
        {
            return !_context.Comments.Any() ? new List<Comment>() : _context.Comments.ToList();
        }

        public void Update(Comment entity)
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