using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Infrastructure;

namespace ItHappened.Application
{
    public class TrackerService : ITrackerService
    {
        public TrackerService(IRepository<Tracker> repository)
        {
            _repository = repository;
        }
        public Guid CreateTracker(TrackerCreationContent content)
        {
            var id = Guid.NewGuid();
            var tracker = new Tracker(id, content.UserId, content.Title, DateTime.Now, 
                DateTime.Now, content.Customizations);
            _repository.Save(tracker);
            return id;
        }

        public void EditTracker(Guid trackerId, TrackerEditingContent content)
        {
            var oldTracker = _repository.Get(trackerId);
            var newTracker = new Tracker(oldTracker.Id, oldTracker.UserId, content.Title, 
                oldTracker.CreationDate,DateTime.Now, content.Customizations);
            _repository.Update(trackerId, newTracker);
        }

        public void DeleteTracker(Guid trackerId)
        {
            _repository.Delete(trackerId);
        }
        
        private readonly IRepository<Tracker> _repository;
    }
}