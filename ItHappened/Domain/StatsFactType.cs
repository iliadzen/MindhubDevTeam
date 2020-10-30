namespace ItHappened.Domain
{
    public enum StatsFactType
    {
        // Common
        ManyEventsOverall,        // Зафиксировано уже N событий
        BiggestDayOverall,        // Самый насыщенный событиями день
        BiggestWeekOverall,       // Самая насыщенная событиями неделя
        MostFrequentEventOverall, // Самое частое событие
        
        // Specific to tracker
        BestEvent,           // Лучшее событие
        WorstEvent,          // Худшее событие
        ManyEvents,          // Количество событий
        LongestBreak,        // Самый долгий перерыв
        LongGoneEvent,       // Давно не происходило
        AverageRating,       // Среднее значение оценки
        TotalScaleValue,     // Суммарное значение шкалы
        AverageScaleValue,   // Среднее значение шкалы
        EventOnDayOfTheWeek, // Происходит в определённые дни недели
        EventAtTimeOfTheDay, // Происходит в определённое время суток
    }
}