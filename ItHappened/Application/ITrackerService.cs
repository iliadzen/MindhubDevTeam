using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application
{
    public interface ITrackerService
    {
        void CreateTracker(Guid actorId, TrackerCreationContent content);
        void EditTracker(Guid actorId, Guid trackerId, TrackerCreationContent content);
        void DeleteTracker(Guid actorId, Guid trackerId);
        IReadOnlyCollection<Tracker> GetUserTrackers(Guid userId);
        Option<Tracker> GetTracker(Guid actorId, Guid trackerId);
    }
}