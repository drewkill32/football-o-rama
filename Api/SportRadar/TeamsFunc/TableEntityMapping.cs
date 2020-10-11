using System;
using System.Collections.Generic;
using TeamModel = BlazorApp.Shared.Team;
using ConferenceModel = BlazorApp.Shared.Conference;

namespace BlazorApp.Api.SportRadar.TeamsFunc
{
    public static class TableEntityMapping
    {
        public static IEnumerable<TeamTableEntity> ToTeamTableEntites(this TeamsEntity teamsEntity)
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var conference in teamsEntity.Conferences)
            {
                if (conference.Subdivisions.Count == 0)
                {
                    foreach (var team in conference.Teams)
                    {
                        yield return new TeamTableEntity(conference.Id, team.Id)
                        {
                            Timestamp = now,
                            Market = team.Market,
                            Name = team.Name,
                            ConferenceName = conference.Name
                        };
                    }
                }
                else
                {
                    foreach (var subdivision in conference.Subdivisions)
                    {
                        foreach (var team in subdivision.Teams)
                        {
                            yield return new TeamTableEntity(conference.Id, team.Id)
                            {
                                Timestamp = now,
                                Market = team.Market,
                                Name = team.Name,
                                ConferenceName = conference.Name
                            };
                        }
                    }
                }
            }
        }

        public static TeamModel ToTeamModel(TeamTableEntity team)
        {
            return new TeamModel()
            {
                Name = team.Name,
                Market = team.Market,
                Id = team.Id,
                Conference = new ConferenceModel()
                {
                    Name = team.ConferenceName,
                    Id = team.ConferenceId
                }
            };
        }
    }
}