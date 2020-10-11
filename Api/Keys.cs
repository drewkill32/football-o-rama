using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Api
{
    public static class Keys
    {

        public  class SportsRadar
        {
            public const string ApiKey = "SportRadarApiKey";
            public const string Name = "SportRadar";
            public const string TeamsUrlFormat = "/ncaafb-t1/teams/FBS/hierarchy.json?api_key={0}";
            public const string ScheduleUrlFormat = "/ncaafb-t1/{0}/REG/schedule.json?api_key={1}";
        }

        public static class Azure
        {
            public const string AppInsightsKey = "APPINSIGHTS_INSTRUMENTATIONKEY";
            public const string Teams = "Teams";
            public const string ConnectionStringKey = "AzureWebJobsStorage";
            public const string Schedule = "Schedule";
            public const string Week = "Week";
        }
        
    }
}
