using System;
using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public interface IRepository<T>
    {
        void Save(T entity);
        Option<T> Get(Guid id);
        IReadOnlyCollection<T> GetAll();
        void Update(T entity);
        void Delete(Guid id);
    }
}