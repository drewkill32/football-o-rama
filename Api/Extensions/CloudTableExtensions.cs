using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace BlazorApp.Api.Extensions
{
    public static class CloudTableExtensions
    {

        public static async Task InsertOrMergeAll(this CloudTable table, IEnumerable<TableEntity> entities)
        {
            var tasks = entities
                .Select(TableOperation.InsertOrMerge)
                .Select(table.ExecuteAsync)
                .Cast<Task>()
                .ToArray();

            await Task.WhenAll(tasks);
        }
    }
}
