using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Api.SportRadar
{
    [StorageAccount(Keys.Azure.AzureStorage)]
    public class SportRadarBase
    {
        protected HttpClient HttpClient;
        protected string ApiKey;

        public SportRadarBase(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            HttpClient = httpClientFactory.CreateClient(Keys.SportsRadar.Name);
            ApiKey = configuration[Keys.SportsRadar.ApiKey];
        }
    }
}