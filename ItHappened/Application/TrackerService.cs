using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Application
{
    public class TrackerService : ITrackerService
    {
        private readonly IRepository<Tracker> _trackersRepository;
        public TrackerService(IRepository<Tracker> trackersRepository)
        {
            _trackersRepository = trackersRepository;
        }
        public void CreateTracker(Guid actorId, TrackerCreationContent content)
        {
            if (!CanCreateTracker(actorId)) {
                Logger.Error("User can't create tracker");
                return;
            }
            var trackerId = Guid.NewGuid();
            var tracker = new Tracker(trackerId, actorId, content.Title, DateTime.Now, 
                DateTime.Now, content.Customizations);
            _trackersRepository.Save(tracker);
        }

        public void EditTracker(Guid actorId, Guid trackerId, TrackerCreationContent content)
        {
            if (!CanEditTracker(actorId, trackerId)) {
                Logger.Error("User tried to edit someone else's tracker");
                return;
            }
            var oldTracker = _trackersRepository.Get(trackerId);
            oldTracker.Do(tracker =>
            {
                var newTracker = new Tracker(tracker.Id, tracker.UserId, content.Title,
                    tracker.CreationDate, DateTime.Now, content.Customizations);
                _trackersRepository.Update(newTracker);
            });
        }

        public void DeleteTracker(Guid actorId, Guid trackerId)
        {
            if (!CanDeleteTracker(actorId, trackerId)) {
                Logger.Error("User tried to edit someone else's tracker");
                return;
            }
            _trackersRepository.Delete(trackerId);
        }
        
        public IReadOnlyCollection<Tracker> GetUserTrackers(Guid userId)
        {
            var trackers = _trackersRepository.GetAll();
            return trackers.Where(tracker => tracker.UserId == userId) as IReadOnlyCollection<Tracker>;
        }

        public Option<Tracker> GetTracker(Guid actorId, Guid trackerId)
        {
            if (!CanGetTracker(actorId, trackerId))
            {
                Logger.Error("User tried to get someone else's tracker");
                return Option<Tracker>.None;
            }
            return _trackersRepository.Get(trackerId);
        }

        private bool CanCreateTracker(Guid userId)
        {
            return true;
        }
        
        private bool CanGetTracker(Guid userId, Guid trackerId)
        {
            var tracker = _trackersRepository.Get(trackerId);
            if (tracker.IsNone) return false;
            return tracker.ValueUnsafe().UserId == userId;
        }
        
        private bool CanEditTracker(Guid userId, Guid trackerId)
        {
            var tracker = _trackersRepository.Get(trackerId);
            if (tracker.IsNone) return false;
            return tracker.ValueUnsafe().UserId == userId;
        }
        
        private bool CanDeleteTracker(Guid userId, Guid trackerId)
        {
            var tracker = _trackersRepository.Get(trackerId);
            if (tracker.IsNone) return false;
            return tracker.ValueUnsafe().UserId == userId;
        }
    }
}