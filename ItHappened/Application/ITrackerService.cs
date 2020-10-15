using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public interface ITrackerService
    {
        Guid CreateTracker(TrackerCreationContent content);
        void EditTracker(Guid trackerId, TrackerCreationContent content);
        void DeleteTracker(Guid userId, Guid trackerId);
        IReadOnlyCollection<Tracker> GetUserTrackers(Guid userId);
        Tracker GetTracker(Guid trackerId);
    }
}