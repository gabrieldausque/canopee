using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using Canopee.Common;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms.Databases.Firebird
{
    [Export("FirebirdInsert", typeof(ITransform))]
    public class FirebirdInsertFieldsTransform : ITransform
    {
        private string _connectionString;
        private string _selectStatement;
        private List<TransformFieldMapping> _requestedFieldMappings;

        public FirebirdInsertFieldsTransform()
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
                    foreach (var mapping in _requestedFieldMappings)
                    {
                        input.SetFieldValue(mapping.LocalName, row[mapping.SearchedName]);
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