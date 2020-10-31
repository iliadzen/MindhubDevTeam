using System;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public interface ICustomizationService
    {
        void AddEventCustomizationData(Guid actorId, Guid eventId, EventCustomizationData data);
        void AddTrackerCustomization(Guid actorId, Guid trackerId, CustomizationType type);
    }
}