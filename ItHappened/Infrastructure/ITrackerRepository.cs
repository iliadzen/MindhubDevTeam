using System;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public interface ITrackerRepository
    {
        Guid SaveTracker(Tracker tracker);
        Tracker GetTracker(Guid trackerId);
        void UpdateTracker(Guid trackerId, Tracker newTracker);
        void DeleteTracker(Guid trackerId);
    }
}