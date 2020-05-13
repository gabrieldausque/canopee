using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using System.Security.Cryptography.Xml;
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
    /// This transforms will make a lookup for a specific key field in a recordset from a select in a firebird database, and add expected field in the <see cref="ICollectedEvent"/>
    ///
    /// The configuration will be :
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
    ///                            "TransformType": "FirebirdLookup",
    ///                            "ConnectionString": "User=mylogin;Password=mypassword;Database=database.fdb;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;",
    ///                            "SelectStatement": "SELECT * FROM userinfos",
    ///                            "Key" : {
    ///                                 "LocalName":"KeyNameInCollectedEvent",
    ///                                 "SearchedName":"KeyNameInRecordSet"
    ///                             }, 
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
    /// The TransformType attribute will be FirebirdLookup
    /// The ConnectionString attribute will contain the connection string to the database
    /// The SelectStatement attribute will contain the select statement that will contain the wanted fields
    /// The Key attribute will contain a mapping definition for the key field : LocalName is the name of the key in the <see cref="ICollectedEvent"/>, SearchedName is the name of the key in the record set 
    /// The Fields attribute will contain all mappings for each wanted field : LocalName attribute will define the name of the field in the <see cref="ICollectedEvent"/>, SearchedName will define the name of the field in the recordset from the select statement
    ///  
    /// </summary>
    [Export("FirebirdLookup", typeof(ITransform))]
    public class FirebirdLookupTransform : BaseTransform
    {
        /// <summary>
        /// The connection string to connect to the firebird database
        /// </summary>
        private string _connectionString;
        
        /// <summary>
        /// The select statement to get data from 
        /// </summary>
        private string _selectStatement;
        
        /// <summary>
        /// The key field used for the lookup
        /// </summary>
        private TransformFieldMapping _key;
        
        /// <summary>
        /// The list of field to add
        /// </summary>
        private readonly List<TransformFieldMapping> _requestedFieldMappings;

        /// <summary>
        /// Default constructor. Initialized the list of mapping
        /// </summary>
        public FirebirdLookupTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
        
        /// <summary>
        /// Search the value of the <see cref="_key"/> field in the record set getted from the <see cref="_selectStatement"/>
        /// </summary>
        /// <param name="collectedEventToTransform"><see cref="ICollectedEvent"/> to modify</param>
        /// <returns>the modified <see cref="ICollectedEvent"/></returns>
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
                    if (row[_key.SearchedName] == collectedEventToTransform.GetFieldValue(_key.LocalName))
                    {
                        foreach (var mapping in _requestedFieldMappings)
                        {
                            collectedEventToTransform.SetFieldValue(mapping.LocalName, row[mapping.SearchedName]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while transforming : {ex}");
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