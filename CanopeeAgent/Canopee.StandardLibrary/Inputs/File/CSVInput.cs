using System;
using Canopee.Common;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;

namespace Canopee.StandardLibrary.Inputs.File
{
    /// <summary>
    /// Work In Progress.
    /// Collect all line of a CSV file and get them.
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
    ///                         "InputType": "CSV",
    ///                         "File": "./AgentLocation.csv",
    ///                         "FileType": "CSV",
    ///                         "FieldSeparator": ";",
    ///                         "WithHeader": true
    ///                     }
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
    /// The InputType is CSV.
    /// The File element define the file to load
    /// The FileType define the algo used to read the file. Currently only CSV is available
    /// The FieldSeparator define the field separator of the current csv
    /// WithHeader indicate if the file has header. Today this option must be set to true. 
    /// 
    /// </summary>
    [Export("CSV", typeof(IInput))]
    public class CSVInput : BaseInput
    {
        /// <summary>
        /// The file path to extract event from
        /// </summary>
        private string _filePath;
        
        /// <summary>
        /// the internal fileInfo that give information on the file (creation date, modification date, etc ...)
        /// </summary>
        private FileVersionInfo _fileInfo;
        
        /// <summary>
        /// the file type
        /// </summary>
        private string _fileType;
        
        /// <summary>
        /// indicate if the file has headers
        /// </summary>
        private bool _withHeader;
        
        /// <summary>
        /// The field separator for the file
        /// </summary>
        private string _fieldSeparator;

        /// <summary>
        /// Initialize the <see cref="IInput"/> using the input configuration. Set all internal properties and initialize the logger
        /// </summary>
        /// <param name="inputConfiguration">the input configuration</param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        /// <param name="agentId">the agent id that will be set in all <see cref="ICollectedEvent"/></param>
        public override void Initialize(IConfigurationSection inputConfiguration, IConfigurationSection loggingConfiguration, string agentId)
        {
            base.Initialize(inputConfiguration, loggingConfiguration, agentId);
            _filePath = Path.GetFullPath(inputConfiguration["File"]);
            _fileInfo = FileVersionInfo.GetVersionInfo(_filePath);
            _fileType = inputConfiguration["FileType"];
           
            bool.TryParse(inputConfiguration["WithHeader"], out _withHeader);
            _fieldSeparator = inputConfiguration["FieldSeparator"];
            
            //TODO : initialize the latest position for the stream to be opened on collect
            //TODO : initialize the check when the file has been created 
            //TODO : save data in temporary file for restarting when 
        }

        /// <summary>
        /// Collect all record in the CSV file as <see cref="ICollectedEvent"/>.
        /// For now, no headers csv file are not managed.
        /// </summary>
        /// <param name="fromTriggerEventArgs">the event arg that is send by the trigger</param>
        /// <returns>a collection of event corresponding to each line of the csv file</returns>
        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            Logger.LogDebug($"Collecting events from {_filePath}");
            try
            {
                var collectedEvents = new List<ICollectedEvent>();
                var headers = new List<string>();
                using(FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
                using(StreamReader sr = new StreamReader(fs))
                {
                    int lineIndex = 0;
                    while (!sr.EndOfStream)
                    {
                        if(_withHeader && lineIndex == 0)
                        {
                            headers.AddRange(sr.ReadLine().Split(_fieldSeparator));
                            lineIndex++;
                        } 
                        else
                        {
                            var line = sr.ReadLine();
                            var info = new RawFileLineInfo(AgentId)
                            {
                                Raw = line
                            };
                            if (_withHeader && _fileType.ToUpper() == "CSV") {
                                var fields = line.Split(_fieldSeparator);
                                for(int fieldIndex=0;fieldIndex < headers.Count; fieldIndex++)
                                {
                                    if (!string.IsNullOrWhiteSpace(headers[fieldIndex]))
                                    {
                                        info.SetFieldValue(headers[fieldIndex], fields[fieldIndex]);
                                    }
                                }
                            }
                            collectedEvents.Add(info);
                        }
                    }
                }
                return collectedEvents;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error when collecting events from {_filePath} : {ex}");
                throw;
            }
                
        }
    }
}
