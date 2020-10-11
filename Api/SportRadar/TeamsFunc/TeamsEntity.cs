using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlazorApp.Api.SportRadar.TeamsFunc
{
    public partial class TeamsEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("conferences")]
        public List<Conference> Conferences { get; set; }
    }

    public  class Conference
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("teams", NullValueHandling = NullValueHandling.Ignore)]
        public List<Team> Teams { get; set; } = new List<Team>();

        [JsonProperty("subdivisions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Conference> Subdivisions { get; set; } = new List<Conference>();
    }

    public  class Team
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }


    }

    

    public  partial class TeamsEntity
    {
        public static TeamsEntity FromJson(string json) => JsonConvert.DeserializeObject<TeamsEntity>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TeamsEntity self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    



   
}
