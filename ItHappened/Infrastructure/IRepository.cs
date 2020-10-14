using System;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public interface IRepository<T>
    {
        Guid Save(T entity);
        T Get(Guid id);
        void Update(Guid id, T entity);
        void Delete(Guid id);
    }
}