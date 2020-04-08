using Canopee.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;

namespace Canopee.StandardLibrary.Transforms
{
    [Export("APIGeoGouv",typeof(ITransform))]
    public class APIGeoGouvTransform : BaseTransform
    {
        private static readonly string Url = "https://geo.api.gouv.fr/";
        private string _entity = "communes";
        private TransformFieldMapping _key;

        public override void Initialize(IConfigurationSection transformConfiguration)
        {
            _key = new TransformFieldMapping()
            {
                LocalName = transformConfiguration["Key:LocalName"],
                SearchedName = transformConfiguration["Key:SearchedName"]
            };
        }

        public override ICollectedEvent Transform(ICollectedEvent input)
        {
            try
            {
                UriBuilder builder = new UriBuilder(Url);
                builder.Path = _entity;
                builder.Query = $"{_key.SearchedName}={input.GetFieldValue(_key.LocalName)}&&fields=centre,codeDepartement,codeRegion&format=json&geometry=centre";
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
                            input.SetFieldValue("Location", new
                            {
                                lat = lat,
                                lon = lon
                            });
                            input.SetFieldValue("CodeDepartement", int.Parse(element.GetProperty("codeDepartement").GetString()));
                            input.SetFieldValue("CodeRegion", int.Parse(element.GetProperty("codeRegion").GetString()));
                        }
                    }
                }
                return input;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while transforming : {ex}");
                throw;
            }
        }
    }
}
