using BlazorApp.Shared;
using System.Collections.Generic;
using System.Linq;
using WeekModel = BlazorApp.Shared.Week;

namespace BlazorApp.Api.SportRadar.ScheduleFunc
{
    public static class ScheduleEntityMapping
    {

        public static IEnumerable<GameTableType> ToGameTableEntity(this ScheduleEntity schedule)
        {
            foreach (var week in schedule.Weeks)
            {
                foreach (var game in week.Games)
                {
                    yield return new GameTableType()
                    {
                        RowKey = game.Id.ToString(),
                        Scheduled = game.Scheduled,
                        Home =  game.Home,
                        Away = game.Away,
                        Status = game.Status,
                        HomePoints = game.HomePoints,
                        AwayPoints = game.AwayPoints,
                        //Week
                        PartitionKey = week.Id.ToString(),
                        WeekNumber = week.Number,
                        //season
                        Season = schedule.Season.ToString(),
                        Type = schedule.Type
                    };
                }
            }
        }

        public static IEnumerable<WeekTableType> ToWeekTableEntity(this ScheduleEntity schedule)
        {
            return schedule.Weeks.Select(week => new WeekTableType()
            {
                RowKey = week.Id.ToString(),
                PartitionKey = schedule.Season.ToString(),
                Type = schedule.Type,
                Number = week.Number
            });
        }

        public static IEnumerable<WeekModel> ToWeeksModel(this IEnumerable<GameTableType> games)
        {
            var dict = new Dictionary<string, WeekModel>();
            foreach (var game in games)
            {
                if (dict.TryGetValue(game.WeekId, out var week))
                {
                    week.Games.Add(game.Map());
                }
                else
                {
                    week = new WeekModel()
                    {
                        Id = game.WeekId,
                        Number = game.WeekNumber,
                        Season = game.Season,
                        Type = game.Type
                    };
                    week.Games.Add(game.Map());
                    dict.Add(game.WeekId, week);
                }
            }

            return dict.Values;

        }

        private static Shared.Game Map(this GameTableType game)
        {
            return new Shared.Game()
            {
                Scheduled = game.Scheduled,
                Home = game.Home,
                HomePoints = game.HomePoints,
                Away = game.Away,
                AwayPoints = game.AwayPoints,
                Id = game.Id,
                Status = game.Status
            };
        }
    }
}
