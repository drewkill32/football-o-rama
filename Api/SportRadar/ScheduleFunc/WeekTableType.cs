using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace BlazorApp.Api.SportRadar.ScheduleFunc
{
    public class WeekTableType: TableEntity
    {
        
        public string Season => PartitionKey;


        public string Id => RowKey;

        public string Type { get; set; }

        public int Number { get; set; }
    }
}
