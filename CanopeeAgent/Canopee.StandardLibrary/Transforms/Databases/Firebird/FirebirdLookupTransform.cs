using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using System.Security.Cryptography.Xml;
using Canopee.Common;
using Canopee.Common.Events;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms.Databases.Firebird
{
    [Export("FirebirdLookup", typeof(ITransform))]
    public class FirebirdLookupTransform : ITransform
    {
        private string _connectionString;
        private string _selectStatement;
        private TransformFieldMapping _key;
        private readonly List<TransformFieldMapping> _requestedFieldMappings;

        public FirebirdLookupTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
        
        public ICollectedEvent Transform(ICollectedEvent input)
        {
            FbDataAdapter da = null;
            try
            {
                DataTable dt = new DataTable();
                da = new FbDataAdapter(_selectStatement, _connectionString);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    if (row[_key.SearchedName] == input.GetFieldValue(_key.LocalName))
                    {
                        foreach (var mapping in _requestedFieldMappings)
                        {
                            input.SetFieldValue(mapping.LocalName, row[mapping.SearchedName]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO : log error
            }
            finally
            {
                da?.Dispose();
            }

            return input;
        }

        public void Initialize(IConfigurationSection transformConfiguration)
        {
            _connectionString = transformConfiguration["ConnectionString"];
            _selectStatement = transformConfiguration["SelectStatement"];
            _key = new TransformFieldMapping()
            {
                LocalName = transformConfiguration["Key:LocalName"],
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
    }
}