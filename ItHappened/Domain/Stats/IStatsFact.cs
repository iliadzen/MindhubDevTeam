using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Stats
{
    public interface IStatsFact
    {
        StatsFactType Type { get; }
        string Description { get; }
    }
}