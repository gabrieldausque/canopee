using System;
using System.Collections.Generic;
using System.Composition;
using System.Data.Common;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Inputs.Databases.Firebird
{
    [Export("FireBirdADO", typeof(IInput))]
    public class FirebirdADOInput : BaseInput
    {
        private string _connectionString;
        private string _selectStatement;

        public override void Initialize(IConfiguration configurationInput, string agentId)
        {
            base.Initialize(configurationInput, agentId);
            _connectionString = configurationInput["ConnectionString"];
            _selectStatement = configurationInput["SelectStatement"];
        }

        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedRows = new List<ICollectedEvent>();
            FbConnection connection = null;
            FbCommand command = null;
            FbDataReader reader = null;
            try
            {
                connection = new FbConnection(_connectionString);
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = _selectStatement;
                reader = command.ExecuteReader();
                var columns = reader.GetColumnSchema();
                while (reader.Read())
                {
                    var collectedEvent = new CollectedEvent(AgentId);
                    foreach (var column in columns)
                    {
                        collectedEvent.SetFieldValue(column.ColumnName, reader.GetValue(column.ColumnOrdinal.Value));
                        collectedRows.Add(collectedEvent);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //TODO : log error
            }
            finally
            {
                reader?.Dispose();
                command?.Dispose();
                connection?.Dispose();
            }

            return collectedRows;
        }
    }
}