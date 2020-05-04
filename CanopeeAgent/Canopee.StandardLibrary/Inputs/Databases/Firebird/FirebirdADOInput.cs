using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using System.Data.Common;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Inputs.Databases.Firebird
{
    /// <summary>
    /// Create a collected event for each record returned by the select statement
    ///
    /// Configuration example :
    ///
    /// <example>
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "Products",
    ///                     ...
    ///                     "Input": {
    ///                        "InputType": "FireBirdADO",
    ///                        "ConnectionString": "User=auser;Password=apassword;Database=pathtodatabase;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;",
    ///                        "SelectStatement": "SELECT * FROM INSTALLATIONLOGS"
    ///                    },
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
    /// you must define the ConnectionString and the SelectStatement to use InputType FirebirdADO
    /// 
    /// </summary>
    [Export("FireBirdADO", typeof(IInput))]
    public class FirebirdADOInput : BaseInput
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private string _connectionString;
        
        /// <summary>
        /// The select statement to get event from
        /// </summary>
        private string _selectStatement;

        /// <summary>
        /// Initialize this <see cref="IInput"/> using the configuration input. Set the connection string and the select statement.
        /// </summary>
        /// <param name="configurationInput">the configuration input</param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        /// <param name="agentId">the agent id that will be set in all <see cref="ICollectedEvent"/></param>
        public override void Initialize(IConfigurationSection configurationInput, IConfigurationSection loggingConfiguration, string agentId)
        {
            base.Initialize(configurationInput, loggingConfiguration, agentId);
            _connectionString = configurationInput["ConnectionString"];
            _selectStatement = configurationInput["SelectStatement"];
        }

        /// <summary>
        /// Collect all record from the select statement using the internal connection string
        /// </summary>
        /// <param name="fromTriggerEventArgs">the event arg gotten from trigger</param>
        /// <returns>a collection of <see cref="ICollectedEvent"/>, each corresponding to a record from the select statement</returns>
        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            Logger.LogDebug($"Getting events from FireBird database");

            var collectedRows = new List<ICollectedEvent>();
            FbDataAdapter da = null;
            try
            {
                DataTable dt = new DataTable();
                da = new FbDataAdapter(_selectStatement, _connectionString);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    var newEvent = new CollectedEvent(AgentId);
                    foreach (DataColumn col in dt.Columns)
                    {
                        newEvent.SetFieldValue(col.ColumnName, row[col]);
                    }
                    collectedRows.Add(newEvent);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while getting events from database : {ex}");
            }
            finally
            {
                da?.Dispose();
            }
            return collectedRows;
        }
    }
}