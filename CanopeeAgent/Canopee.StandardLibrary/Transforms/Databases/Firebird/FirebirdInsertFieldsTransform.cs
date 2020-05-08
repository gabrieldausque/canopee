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
    /// <summary>
    /// This transforms will add all fields configured in the mapping section to the <see cref="ICollectedEvent"/>
    ///
    /// Configuration will be :
    ///
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
    ///                            "TransformType": "FirebirdInsert",
    ///                            "ConnectionString": "User=mylogin;Password=mypassword;Database=database.fdb;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;",
    ///                            "SelectStatement": "SELECT * FROM userinfos",
    ///                            "Fields": [
    ///                            {
    ///                                "LocalName": "Field1NewName",
    ///                                "SearchedName": "Field1"
    ///                            },
    ///                            {
    ///                                "LocalName": "Field2NewName",
    ///                                "SearchedName": "Field2"
    ///                            }
    ///                         }
    ///                     ]
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }    /// 
    /// </code>
    /// </example>
    ///
    /// The TransformType attribute will be FirebirdInsert
    /// The ConnectionString attribute will contain the connection string to the database
    /// The SelectStatement attribute will contain the select statement that will contain the wanted fields
    /// The Fields attribute will contain all mappings for each wanted field : LocalName attribute will define the name of the field in the <see cref="ICollectedEvent"/>, SearchedName will define the name of the field in the recordset from the select statement
    /// </summary>
    [Export("FirebirdInsert", typeof(ITransform))]
    public class FirebirdInsertFieldsTransform : BaseTransform
    {
        /// <summary>
        /// The firebird connection string
        /// </summary>
        private string _connectionString;
        
        /// <summary>
        /// The select statement
        /// </summary>
        private string _selectStatement;
        
        /// <summary>
        /// The list of field mapping for field integration
        /// </summary>
        private List<TransformFieldMapping> _requestedFieldMappings;

        /// <summary>
        /// Default constructor. Initialized the list of field mappings
        /// </summary>
        public FirebirdInsertFieldsTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
        
        /// <summary>
        /// Add new fields from the SelectStatement to a collected event
        /// </summary>
        /// <param name="collectedEventToTransform">the <see cref="ICollectedEvent"/> to modify</param>
        /// <returns>the <see cref="ICollectedEvent"/> to enrich</returns>
        public override ICollectedEvent Transform(ICollectedEvent collectedEventToTransform)
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
                        collectedEventToTransform.SetFieldValue(mapping.LocalName, row[mapping.SearchedName]);
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
            return collectedEventToTransform;        
        }

        /// <summary>
        /// Initialize this <see cref="ITransform"/> with configurations. Add all defined <see cref="TransformFieldMapping"/> from the configuration.
        /// </summary>
        /// <param name="transformConfiguration">The transform configuration</param>
        /// <param name="loggingConfiguration">The logger configuration</param>
        public override void Initialize(IConfigurationSection transformConfiguration, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(transformConfiguration, loggingConfiguration);
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