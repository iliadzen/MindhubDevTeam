using System;

namespace ItHappened.Domain.Customizations
{
    public interface IEventCustomizationData
    {
        Guid EventId { get; }
    }
}