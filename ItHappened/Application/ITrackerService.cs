using System;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public interface ITrackerService
    {
        Guid CreateTracker(TrackerCreationContent content);
        void DeleteTracker(Guid trackerId);
    }
}