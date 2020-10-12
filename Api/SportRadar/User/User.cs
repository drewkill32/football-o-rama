using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorApp.Api.Extensions;
using BlazorApp.Api.SportRadar.ScheduleFunc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorApp.Api.SportRadar.User
{
    public class Users:SportRadarBase
    {
        public Users(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory, configuration)
        {
        }

        [FunctionName("User")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequest req,
            [Table(Keys.Azure.User,Keys.Azure.User)] CloudTable userTable,
            ILogger log)
        {

            
            try
            {
                var user = req.GetIdentity();
                if (user == null || !user.IsInRole("authenticated"))
                {
                    return new UnauthorizedResult();
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userEntity = JsonConvert.DeserializeObject<UserTableEntity>(requestBody);
                userEntity.RowKey = user.UserId;
                userEntity.Email = user.UserDetails;

                await userTable.ExecuteAsync(TableOperation.InsertOrReplace(userEntity));
                return new OkObjectResult(new
                {
                    userEntity.Name,
                    userEntity.Team,
                    userEntity.UserName,
                });
            }
            catch (Exception e)
            {
                log.LogError(e, "Error getting result. {ErrorMessage}", e.Message);
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
    }
}
