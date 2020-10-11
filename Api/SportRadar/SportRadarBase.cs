using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Api.SportRadar
{
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