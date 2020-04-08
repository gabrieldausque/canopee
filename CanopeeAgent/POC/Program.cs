using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Canopee.Common.Pipelines.Events;

namespace POC
{
    public class WeatherForecast
    {
       
        [JsonExtensionData]
        public Dictionary<string, object> ExtractedFields { get; set; }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            string json =
                "{ \"Date\": \"2019-08-01T00:00:00-07:00\",\"temperatureCelsius\": 25,\"Summary\": \"Hot\",\"DatesAvailable\": [\"2019-08-01T00:00:00-07:00\",\"2019-08-02T00:00:00-07:00\"],\"SummaryWords\": [\"Cool\",\"Windy\",\"Humid\"]}";
            var newObject = JsonSerializer.Deserialize<CollectedEvent>(json);
        }
    }
}