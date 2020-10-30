using LanguageExt;

namespace ItHappened.Domain.Stats
{
    public interface IStatsFact
    {
        StatsFactType Type { get; }
        double Priority { get; }
        string Description { get; }

        Option<IStatsFact> Apply(Event[] events);
    }
}