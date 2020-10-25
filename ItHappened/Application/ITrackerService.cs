using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application
{
    public interface ITrackerService
    {
        void CreateTracker(Guid actorId, TrackerForm form);
        void EditTracker(Guid actorId, Guid trackerId, TrackerForm form);
        void DeleteTracker(Guid actorId, Guid trackerId);
        IReadOnlyCollection<Tracker> GetUserTrackers(Guid userId);
        Option<Tracker> GetTracker(Guid actorId, Guid trackerId);
    }
}