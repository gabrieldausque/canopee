using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.StandardLibrary.Transforms.Databases.Firebird
{
    [Export("FirebirdInsert", typeof(ITransform))]
    public class FirebirdInsertFieldsTransform : BaseTransform
    {
        private string _connectionString;
        private string _selectStatement;
        private List<TransformFieldMapping> _requestedFieldMappings;

        public FirebirdInsertFieldsTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
        public override ICollectedEvent Transform(ICollectedEvent input)
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
                Logger.LogError($"Error while getting datas from Firebird database : {ex}");
                throw;
            }
            finally
            {
                da?.Dispose();
            }
            return input;        
        }

        public override void Initialize(IConfigurationSection transformConfiguration)
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