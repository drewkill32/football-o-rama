﻿using BlazorApp.Api.Extensions;
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

namespace BlazorApp.Api.SportRadar.ScheduleFunc
{
    public class Schedule : SportRadarBase
    {
        public Schedule(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory, configuration)
        {
        }

        [FunctionName("Schedule")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "schedule")] HttpRequest req,
            [Table(Keys.Azure.Schedule, Connection = Keys.Azure.ConnectionStringKey)] CloudTable scheduleTable,
            ILogger log)
        {
            try
            {
                var query = new TableQuery<GameTableType>() { TakeCount = 1000 };
                var scheduleTableEntities = (await scheduleTable.ExecuteQuerySegmentedAsync(query, null)).Results;

                var models = scheduleTableEntities.ToWeeksModel();
                return new OkObjectResult(models);
            }
            catch (Exception e)
            {
                log.LogError(e, "Error gettings result. {ErrorMessage}", e.Message);
                return new InternalServerErrorResult();
            }
        }

        [FunctionName("ScheduleUpdate")]
        public async Task<IActionResult> RunPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "schedule/update")] HttpRequest req,
            [Table(Keys.Azure.Schedule, Connection = Keys.Azure.ConnectionStringKey)] CloudTable scheduleTable,
            [Table(Keys.Azure.Week, Connection = Keys.Azure.ConnectionStringKey)] CloudTable weekTable,
            ILogger log)
        {
            try
            {
                var radarSchedule = await GetSportRadarSchedule();
                var weekEntities = GetWeeks(radarSchedule);
                var scheduleTableEntities = GetGames(radarSchedule);
                await Task.WhenAll(scheduleTable.InsertOrMergeAll(scheduleTableEntities), weekTable.InsertOrMergeAll(weekEntities));

                var models = scheduleTableEntities.ToWeeksModel();
                return  new CreatedResult("schedule",models);
            }
            catch (Exception e)
            {
                log.LogError(e, "Error gettings result. {ErrorMessage}", e.Message);
                return new InternalServerErrorResult();
            }
        }

        private List<GameTableType> GetGames(ScheduleEntity radarSchedule)
        {
            return radarSchedule.ToGameTableEntity().ToList();
        }

        private List<WeekTableType> GetWeeks(ScheduleEntity radarSchedule)
        {
            return radarSchedule.ToWeekTableEntity().ToList();
        }

        private async Task<ScheduleEntity> GetSportRadarSchedule()
        {
            var url = string.Format(Keys.SportsRadar.ScheduleUrlFormat, DateTime.Today.Year, ApiKey);
            var json = await HttpClient.GetStringAsync(url);
            var radarSchedule = ScheduleEntity.FromJson(json);
            return radarSchedule;
        }
    }
}