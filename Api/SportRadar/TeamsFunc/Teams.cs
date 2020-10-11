using BlazorApp.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlazorApp.Api.SportRadar.TeamsFunc
{
    public class Teams : SportRadarBase
    {
        public Teams(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory, configuration)
        {
        }

        [FunctionName("Teams")]
        public async Task<IActionResult> RunGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "teams")] HttpRequest req,
            [Table(Keys.Azure.Teams, Connection = Keys.Azure.ConnectionStringKey)] CloudTable teamsTable,
            ILogger log)
        {
            try
            {
                var query = new TableQuery<TeamTableEntity>() { TakeCount = 250 }; //there is only 130 teams. Make sure the query returns all teams in one pass
                var teamTableResults = (await teamsTable.ExecuteQuerySegmentedAsync(query, null)).Results;
                var models = teamTableResults.Select(TableEntityMapping.ToTeamModel);
                return new OkObjectResult(models);
            }
            catch (Exception e)
            {
                log.LogError(e, "Error gettings result. {ErrorMessage}", e.Message);
                return new JsonResult(new
                {
                    Error = e,
                    e.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [FunctionName("TeamsUpdate")]
        public async Task<IActionResult> RunPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "teams/update")] HttpRequest req,
            [Table(Keys.Azure.Teams, Connection = Keys.Azure.ConnectionStringKey)] CloudTable teamsTable,
            ILogger log)
        {
            try
            {
                var teamTableResults = await GetTeamsFromSportsRadar();
                await teamsTable.InsertOrMergeAll(teamTableResults);

                var models = teamTableResults.Select(TableEntityMapping.ToTeamModel);
                return new CreatedResult("teams",models);
            }
            catch (Exception e)
            {
                log.LogError(e, "Error gettings result. {ErrorMessage}", e.Message);
                return new InternalServerErrorResult();
            }
        }

        private async Task<List<TeamTableEntity>> GetTeamsFromSportsRadar()
        {
            var url = string.Format(Keys.SportsRadar.TeamsUrlFormat, ApiKey);
            var json = await HttpClient.GetStringAsync(url);
            var radarTeams = TeamsEntity.FromJson(json);
            var teamTableResults = radarTeams.ToTeamTableEntites().ToList();
            return teamTableResults;
        }
    }
}