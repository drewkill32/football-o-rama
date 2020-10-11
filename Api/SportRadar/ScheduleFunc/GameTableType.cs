using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.Azure.Cosmos.Table;

namespace BlazorApp.Api.SportRadar.ScheduleFunc
{
    public class GameTableType:TableEntity
    {
        

        public string Id => RowKey;


        public string WeekId => PartitionKey;

        public DateTimeOffset Scheduled { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public GameStatus Status { get; set; }

        public int? AwayPoints { get; set; }

        public int? HomePoints { get; set; }

        public string Season { get; set; }

        public string Type { get; set; }
        public int WeekNumber { get; set; }
    }
}
