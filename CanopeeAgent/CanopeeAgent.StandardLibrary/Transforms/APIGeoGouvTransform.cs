using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace CanopeeAgent.StandardIndicators.Transforms
{
    [Export("APIGeoGouv",typeof(ITransform))]
    public class APIGeoGouvTransform : ITransform
    {
        private static readonly string Url = "https://geo.api.gouv.fr/";
        private string _entity = "communes";
        private TransformFieldMapping _key;

        public void Initialize(IConfigurationSection transformConfiguration)
        {
            _key = new TransformFieldMapping()
            {
                LocalName = transformConfiguration["Key:LocalName"],
                SearchedName = transformConfiguration["Key:SearchedName"]
            };
        }

        public ICollectedEvent Transform(ICollectedEvent input)
        {
            UriBuilder builder = new UriBuilder(Url);
            builder.Path = _entity;
            builder.Query = $"{_key.SearchedName}={input.GetFieldValue(_key.LocalName)}&&fields=centre&format=json&geometry=centre";
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(builder.Uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (StreamReader sr = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                    using (JsonDocument document = JsonDocument.Parse(sr.ReadToEnd()))
                    {
                        var element = document.RootElement[0];
                        var lat = element.GetProperty("centre").GetProperty("coordinates")[1].GetDecimal();
                        var lon = element.GetProperty("centre").GetProperty("coordinates")[0].GetDecimal();
                        input.AddExtractedField("Location", new
                        {
                            lat = lat,
                            lon = lon
                        });
                    }
                }
            }
            return input;
        }
    }
}
