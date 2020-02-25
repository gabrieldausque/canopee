using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace CanopeeAgent.StandardIndicators.Transforms
{
    [Export("ElasticVLookup", typeof(ITransform))]
    class ElasticLookupTransform : ITransform
    {
        private string _elasticUrl;
        private string _searchedIndex;
        private TransformFieldMapping _key;
        private List<TransformFieldMapping> _requestedFieldMappings;

        public ElasticLookupTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
        public void Initialize(IConfigurationSection transformConfiguration)
        {
            _elasticUrl = transformConfiguration["Url"];
            _searchedIndex = transformConfiguration["SearchedIndex"];
            _key = new TransformFieldMapping()
            {
                LocalName = transformConfiguration["Key:Local"],
                SearchedName = transformConfiguration["Key:SearchedName"]
            };
            foreach(var field in transformConfiguration.GetSection("Fields").GetChildren())
            {
                _requestedFieldMappings.Add(new TransformFieldMapping()
                {
                    LocalName = field["LocalName"],
                    SearchedName = field["SearchedName"]
                });
            }
        }

        public ICollectedEvent Transform(ICollectedEvent input)
        {
            //TODO : create elastic client and request for the searched fields 
            //TODO : put fields obtained in extendedfields dictionary
            return input;
        }
    }
}
