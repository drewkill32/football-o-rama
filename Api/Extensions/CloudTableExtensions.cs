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

        public static Task InsertOrMergeAll(this CloudTable table, IEnumerable<TableEntity> entities)
        {
            var batchOp = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batchOp.InsertOrReplace(entity);
            }

            return table.ExecuteBatchAsync(batchOp);

        }
    }
}
