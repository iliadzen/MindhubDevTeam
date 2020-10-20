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
        public TrackerService(IRepository<Tracker> trackersRepository)
        {
            _trackersRepository = trackersRepository;
        }
        public void CreateTracker(Guid actorId, TrackerCreationForm form)
        {
            var tracker = new Tracker(Guid.NewGuid(), actorId, form.Title, DateTime.Now, 
                DateTime.Now, form.Customizations);
            _trackersRepository.Save(tracker);
        }

        public void EditTracker(Guid actorId, Guid trackerId, TrackerCreationForm form)
        {
            var oldTracker = _trackersRepository.Get(trackerId);
            oldTracker.Do(tracker =>
            {
                if (actorId != tracker.UserId)
                {
                    Logger.Error($"User {actorId} tried to edit someone else's tracker");
                    return;
                }
                var newTracker = new Tracker(tracker.Id, tracker.UserId, form.Title,
                    tracker.CreationDate, DateTime.Now, form.Customizations);
                _trackersRepository.Update(newTracker);
            });
        }

        public void DeleteTracker(Guid actorId, Guid trackerId)
        {
            var optionTracker = _trackersRepository.Get(trackerId);
            optionTracker.Do(tracker =>
            {
                if (actorId != tracker.UserId)
                {
                    Logger.Error($"User {actorId} tried to delete someone else's tracker");
                    return;
                }
                _trackersRepository.Delete(trackerId);
            });
        }
        
        public IReadOnlyCollection<Tracker> GetUserTrackers(Guid userId)
        {
            var trackers = _trackersRepository.GetAll();
            return (IReadOnlyCollection<Tracker>)trackers.Where(tracker => tracker.UserId == userId);
        }

        public Option<Tracker> GetTracker(Guid actorId, Guid trackerId)
        {
            var optionTracker = _trackersRepository.Get(trackerId);
            return optionTracker.Map(tracker =>
            {
                if (actorId != tracker.UserId)
                {
                    Logger.Error($"User {actorId} tried to get someone else's tracker");
                    return null;
                }
                return tracker;
            });
        }
        
        private readonly IRepository<Tracker> _trackersRepository;
    }
}