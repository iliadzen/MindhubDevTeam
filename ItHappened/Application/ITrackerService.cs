using System;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public interface ITrackerService
    {
        Guid CreateTracker(TrackerCreationContent content);
        void EditTracker(Guid trackerId, TrackerEditingContent content);
        void DeleteTracker(Guid trackerId);
    }
}