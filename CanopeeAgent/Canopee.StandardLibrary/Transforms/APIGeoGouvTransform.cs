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
    /// <summary>
    /// <see cref="ITransform"/> that will add location coordinates, department code and region code in a <see cref="ICollectedEvent"/> based on a specific field that must represent a search field in the  "https://geo.api.gouv.fr/" public API.
    /// 
    /// Configuration will be :
    /// <example>
    /// <code>
    ///
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Transforms" : [
    ///                         {
    ///                            "TransformType": "APIGeoGouv",
    ///                            "Entity":"communes" 
    ///                            "Key": {
    ///                            "LocalName": "PostalCode",
    ///                            "SearchedName":  "codePostal"
    ///                         }
    ///                       ]
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     } 
    /// </code>
    /// </example>
    ///
    /// The TransformType will be APIGeoGouv
    /// The Entity defines the entity to be used in the ApiGeoGouv Api. Defaulted to communes. 
    /// The Key will defines the field to look for in the ApiGeoGouv Api : LocalName will define the name of the field in <see cref="ICollectedEvent"/> to match the SearchedName that will be a search field of the ApiGeoGouv
    ///
    /// In our example, the search is base on the codePostal, and the value is obtained from the PostalCode field of the <see cref="ICollectedEvent"/>; 
    /// 
    /// </summary>
    [Export("APIGeoGouv",typeof(ITransform))]
    public class APIGeoGouvTransform : BaseTransform
    {
        /// <summary>
        /// The api geo gouv url
        /// </summary>
        private static readonly string Url = "https://geo.api.gouv.fr/";
        /// <summary>
        /// The default entity
        /// </summary>
        private string _entity = "communes";
        /// <summary>
        /// The search key field to use for the entity api
        /// </summary>
        private TransformFieldMapping _key;

        /// <summary>
        /// Initialize this <see cref="ITransform"/> with configurations. Add all defined <see cref="TransformFieldMapping"/> from the configuration.
        /// </summary>
        /// <param name="transformConfiguration">The transform configuration</param>
        /// <param name="loggingConfiguration">The logger configuration</param>
        public override void Initialize(IConfigurationSection transformConfiguration, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(transformConfiguration, loggingConfiguration);
            if (!string.IsNullOrWhiteSpace(transformConfiguration["Entity"]))
            {
                _entity = transformConfiguration["Entity"];
            }
            _key = new TransformFieldMapping()
            {
                LocalName = transformConfiguration["Key:LocalName"],
                SearchedName = transformConfiguration["Key:SearchedName"]
            };
        }

        /// <summary>
        /// Add coordinate in form { lat: value, lon: value} CodeDepartement and CodeRegion in the <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="collectedEventToTransform">the <see cref="ICollectedEvent"/> to transform</param>
        /// <returns>the transform <see cref="ICollectedEvent"/></returns>
        public override ICollectedEvent Transform(ICollectedEvent collectedEventToTransform)
        {
            try
            {
                UriBuilder builder = new UriBuilder(Url);
                builder.Path = _entity;
                builder.Query = $"{_key.SearchedName}={collectedEventToTransform.GetFieldValue(_key.LocalName)}&&fields=centre,codeDepartement,codeRegion&format=json&geometry=centre";
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
                            collectedEventToTransform.SetFieldValue("Location", new
                            {
                                lat = lat,
                                lon = lon
                            });
                            collectedEventToTransform.SetFieldValue("CodeDepartement", int.Parse(element.GetProperty("codeDepartement").GetString()));
                            collectedEventToTransform.SetFieldValue("CodeRegion", int.Parse(element.GetProperty("codeRegion").GetString()));
                        }
                    }
                }
                return collectedEventToTransform;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while transforming : {ex}");
                throw;
            }
        }
    }
}
