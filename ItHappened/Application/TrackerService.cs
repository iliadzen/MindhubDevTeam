using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Infrastructure;

namespace ItHappened.Application
{
    public class TrackerService : ITrackerService
    {
        public TrackerService(ITrackerRepository trackerRepository)
        {
            _trackerRepository = trackerRepository;
        }
        public Guid CreateTracker(TrackerCreationContent content)
        {
            var id = Guid.NewGuid();
            var tracker = new Tracker(id, content.UserId, content.Title, DateTime.Now, content.Customizations);
            _trackerRepository.SaveTracker(tracker);
            return id;
        }

        public void EditTracker(Guid trackerId, TrackerEditingContent content)
        {
            var oldTracker = _trackerRepository.GetTracker(trackerId);
            var newTracker = new Tracker(oldTracker.Id, oldTracker.UserId, content.Title, 
                oldTracker.CreationDate,content.Customizations);
            _trackerRepository.UpdateTracker(trackerId, newTracker);
        }

        public void DeleteTracker(Guid trackerId)
        {
            _trackerRepository.DeleteTracker(trackerId);
        }
        
        private readonly ITrackerRepository _trackerRepository;
    }
}