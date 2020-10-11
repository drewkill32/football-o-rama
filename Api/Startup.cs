using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(BlazorApp.Api.Startup))]

namespace BlazorApp.Api
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var appKey = builder.GetContext().Configuration[Keys.Azure.AppInsightsKey];
            builder.Services.AddLogging(c => c.AddApplicationInsights(appKey));
            builder.Services.AddHttpClient(Keys.SportsRadar.Name, c=> c.BaseAddress = new Uri("https://api.sportradar.us"));
        }
    }
}
