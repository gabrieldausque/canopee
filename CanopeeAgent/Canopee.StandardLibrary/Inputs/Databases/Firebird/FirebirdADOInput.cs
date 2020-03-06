using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
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
                //TODO : log error
            }
            finally
            {
                da?.Dispose();
            }

            return collectedRows;
        }
    }
}