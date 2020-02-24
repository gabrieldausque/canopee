using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using CanopeeAgent.Core.Indicators;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CanopeeAgent.StandardIndicators.Indicators.File
{
    public class FileLinesIndicator : BaseIndicator
    {
        private string _filePath;
        private FileVersionInfo _fileInfo;
        

        public override void Initialize(IConfigurationSection configuration)
        {
            base.Initialize(configuration);

            var inputConfiguration = configuration.GetSection("Input");

            _filePath = Path.GetFullPath(inputConfiguration["File"]);
            _fileInfo = FileVersionInfo.GetVersionInfo(_filePath);
            
            //TODO : initialize the latest position for the stream to be opened on collect
            //TODO : initialize the check when the file has been created 
            //TODO : save data in temporary file for restarting when 
        }

        public override ICollection<ICollectedEvent> InternalCollect()
        {
            var collectedEvents = new List<ICollectedEvent>();
            using(FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            using(StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    collectedEvents.Add(new RawFileLineInfo(_agentId));
                }
            }
            return collectedEvents;    
        }
    }
}
