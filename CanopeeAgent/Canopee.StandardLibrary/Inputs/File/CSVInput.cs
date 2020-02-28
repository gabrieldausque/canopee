using Canopee.Common;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;

namespace Canopee.StandardLibrary.Inputs.File
{
    [Export("CSV", typeof(IInput))]
    public class CSVInput : BaseInput
    {
        private string _filePath;
        private FileVersionInfo _fileInfo;
        private string _fileType;
        private bool _withHeader;
        private string _fieldSeparator;

        public override void Initialize(IConfiguration inputConfiguration, string agentId)
        {
            base.Initialize(inputConfiguration, agentId);
            _filePath = Path.GetFullPath(inputConfiguration["File"]);
            _fileInfo = FileVersionInfo.GetVersionInfo(_filePath);
            _fileType = inputConfiguration["FileType"];
           
            bool.TryParse(inputConfiguration["WithHeader"], out _withHeader);
            _fieldSeparator = inputConfiguration["FieldSeparator"];
            
            //TODO : initialize the latest position for the stream to be opened on collect
            //TODO : initialize the check when the file has been created 
            //TODO : save data in temporary file for restarting when 
        }

        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
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
                                if (!string.IsNullOrEmpty(headers[fieldIndex]))
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
    }
}
