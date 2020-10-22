using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Application
{
    public class TrackerService : ITrackerService
    {
        public TrackerService(IRepository<Tracker> trackersRepository)
        {
            _trackersRepository = trackersRepository;
        }
        public void CreateTracker(Guid actorId, TrackerForm form)
        {
            var tracker = new Tracker(Guid.NewGuid(), actorId, form.Title, DateTime.Now, 
                DateTime.Now, form.Customizations);
            _trackersRepository.Save(tracker);
        }

        public void EditTracker(Guid actorId, Guid trackerId, TrackerForm form)
        {
            var oldTracker = _trackersRepository.Get(trackerId);
            oldTracker.Do(tracker =>
            {
                if (actorId != tracker.UserId)
                {
                    Log.Error($"User {actorId} tried to edit someone else's tracker");
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
                    Log.Error($"User {actorId} tried to delete someone else's tracker");
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
            return optionTracker.Match(
                Some: tracker =>
                {
                    if (actorId != optionTracker.ValueUnsafe().UserId)
                    {
                        Log.Error($"User {actorId} tried to get someone else's tracker");
                        return Option<Tracker>.None;
                    }
                    return tracker;
                },
                None: () =>
                {
                    Log.Error($"Tracker {trackerId} not exists");
                    return Option<Tracker>.None;
                });
        }

        private readonly IRepository<Tracker> _trackersRepository;
    }
}