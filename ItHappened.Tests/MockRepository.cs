using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Tests
{
    public class RepositoryMock<T> : IRepository<T>
        where T : IEntity
    {
        public RepositoryMock()
        {
            _repository = new List<T>();
        }

        public void Save(T entity)
        {
            if (!_repository.Contains(entity))
            {
                _repository.Add(entity);
            }
        }

        public Option<T> Get(Guid id)
        {
            return _repository.SingleOrDefault(entity => entity.Id == id);
        }

        public IReadOnlyCollection<T> GetAll()
        {
            return _repository;
        }

        public void Update(T newEntity)
        {
            Option<T> oldEntity = _repository.SingleOrDefault(entity => entity.Id == newEntity.Id);
            oldEntity.Do(entity =>
            {
                _repository.Remove(entity);
                _repository.Add(newEntity);
            });
        }
        public void Delete(Guid id)
        {
            Option<T> optionEntity = _repository.SingleOrDefault(entity => entity.Id == id);
            optionEntity.Do(e =>
            {
                _repository.Remove(e);
            });
        }

        private List<T> _repository;
    }
}