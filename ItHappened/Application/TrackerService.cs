using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Infrastructure;

namespace ItHappened.Application
{
    public class TrackerService : ITrackerService
    {
        public TrackerService(IRepository<Tracker> trackersRepository)
        {
            _trackersRepository = trackersRepository;
        }
        public Guid CreateTracker(TrackerCreationContent content)
        {
            var id = Guid.NewGuid();
            var tracker = new Tracker(id, content.UserId, content.Title, DateTime.Now, 
                DateTime.Now, content.Customizations);
            _trackersRepository.Save(tracker);
            return id;
        }

        public void EditTracker(Guid trackerId, TrackerCreationContent content)
        {

            var oldTracker = _trackersRepository.Get(trackerId);
            oldTracker.Do(oldTracker =>
            {
                if (content.UserId == oldTracker.UserId)
                {
                    var newTracker = new Tracker(oldTracker.Id, oldTracker.UserId, content.Title,
                        oldTracker.CreationDate, DateTime.Now, content.Customizations);
                    _trackersRepository.Update(newTracker);
                }
                else
                {
                    Logger.Error("User tried to edit someone else's tracker");
                }
            });
        }

        public void DeleteTracker(Guid userId, Guid trackerId)
        {
            var tracker = _trackersRepository.Get(trackerId);
            tracker.Do(tracker =>
            {
                if (userId == tracker.UserId)
                {
                    _trackersRepository.Delete(trackerId);
                }
                else
                {
                    Logger.Error("User tried to delete someone else's tracker");
                }
            });
        }
        
        public IReadOnlyCollection<Tracker> GetUserTrackers(Guid userId)
        {
            var trackers = _trackersRepository.GetAll();
            return (IReadOnlyCollection<Tracker>)trackers.Where(tracker => tracker.UserId == userId);
        }

        public Tracker GetTracker(Guid trackerId)
        {
            
            return (Tracker) _trackersRepository.Get(trackerId).Map(tracker => tracker);
        }
        
        private readonly IRepository<Tracker> _trackersRepository;
    }
}