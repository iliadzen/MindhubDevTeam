using System;
using System.Collections.Generic;

namespace ItHappened.Infrastructure
{
    public interface IRepository<T>
    {
        Guid Save(T entity);
        T Get(Guid id);
        IReadOnlyCollection<T> GetAll();
        void Update(Guid id, T entity);
        void Delete(Guid id);
    }
}