using BlazorApp.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BlazorApp.Api.SportRadar.ScheduleFunc
{
    public partial class ScheduleEntity
    {
        [JsonProperty("season")]
        public int Season { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("weeks")]
        public List<Week> Weeks { get; set; }
    }

    public partial class Week
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("games")]
        public List<Game> Games { get; set; }
    }

    public partial class Game
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("home")]
        public string Home { get; set; }

        [JsonProperty("away")]
        public string Away { get; set; }

        [JsonProperty("status")]
        public GameStatus Status { get; set; }

        [JsonProperty("home_points", NullValueHandling = NullValueHandling.Ignore)]
        public int? HomePoints { get; set; }

        [JsonProperty("away_points", NullValueHandling = NullValueHandling.Ignore)]
        public int? AwayPoints { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,

            Converters =
            {
                StatusConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    public partial class ScheduleEntity
    {
        public static ScheduleEntity FromJson(string json) => JsonConvert.DeserializeObject<ScheduleEntity>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ScheduleEntity self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal class StatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(GameStatus) || t == typeof(GameStatus?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "cancelled":
                    return GameStatus.Cancelled;

                case "closed":
                    return GameStatus.Closed;

                case "complete":
                    return GameStatus.Complete;

                case "created":
                    return GameStatus.Created;

                case "inprogress":
                    return GameStatus.Inprogress;

                case "postponed":
                    return GameStatus.Postponed;

                case "scheduled":
                    return GameStatus.Scheduled;

                case "time-tbd":
                    return GameStatus.TimeTbd;
            }
            throw new Exception("Cannot unmarshal type Status");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (GameStatus)untypedValue;
            switch (value)
            {
                case GameStatus.Cancelled:
                    serializer.Serialize(writer, "cancelled");
                    return;

                case GameStatus.Closed:
                    serializer.Serialize(writer, "closed");
                    return;

                case GameStatus.Complete:
                    serializer.Serialize(writer, "complete");
                    return;

                case GameStatus.Created:
                    serializer.Serialize(writer, "created");
                    return;

                case GameStatus.Inprogress:
                    serializer.Serialize(writer, "inprogress");
                    return;

                case GameStatus.Postponed:
                    serializer.Serialize(writer, "postponed");
                    return;

                case GameStatus.Scheduled:
                    serializer.Serialize(writer, "scheduled");
                    return;

                case GameStatus.TimeTbd:
                    serializer.Serialize(writer, "time-tbd");
                    return;
            }
            throw new Exception("Cannot marshal type Status");
        }

        public static readonly StatusConverter Singleton = new StatusConverter();
    }
}