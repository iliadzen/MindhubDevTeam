using System;
using System.Collections.Generic;
using Serilog;

namespace ItHappened.Domain
{
    public class EventCustomization
    {
        public Guid EventId { get; }
        public EventCustomization(Guid eventId, EventCustomizationData customization)
        {
            EventId = eventId;
            _customizations.Add(customization);
        }

        public void AddCustomization(EventCustomizationData customization)
        {
            if(CheckIfCustomizationOfTheSameTypeInCollection(customization))
                _customizations.Add(customization);
        }

        private bool CheckIfCustomizationOfTheSameTypeInCollection(EventCustomizationData addingCustomization)
        {
            foreach (var customization in _customizations)
            {
                if (addingCustomization.GetType() == customization.GetType())
                {
                    Log.Error($"Customization of type {addingCustomization.GetType()} " +
                                 $"is already exists in event {EventId}");
                    return false;
                }
            }

            return true;
        }
        
        public IReadOnlyCollection<EventCustomizationData> GetCustomizations()
        {
            return _customizations;
        }
        
        private List<EventCustomizationData> _customizations = new List<EventCustomizationData>();
    }
}