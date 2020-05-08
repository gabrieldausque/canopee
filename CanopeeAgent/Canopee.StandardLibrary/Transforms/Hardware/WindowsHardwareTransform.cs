using System.Composition;
using System.Text.RegularExpressions;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Transforms.Hardware
{
    [Export("HardwareWINDOWS", typeof(ITransform))]
    public class WindowsHardwareTransform : BaseHardwareTransform
    {
        public override ICollectedEvent Transform(ICollectedEvent collectedEventToTransform)
        {
            var output = GetBatchOutput("echo architecture=%PROCESSOR_ARCHITECTURE% model=%PROCESSOR_IDENTIFIER% ");
            var regex = new Regex("architecture=(?<architecture>.+)[ ]+model=(?<model>.+)");
            var match = regex.Match(output[0]);
            collectedEventToTransform.SetFieldValue("CpuArchitecture", match.Groups["architecture"].Value);
            collectedEventToTransform.SetFieldValue("CpuModel", match.Groups["model"].Value);
            output = GetBatchOutput("wmic cpu get numberoflogicalprocessors /value ");
            foreach(var  line in output)
            {
                if (line.Contains("NumberOfLogicalProcessors"))
                {
                    collectedEventToTransform.SetFieldValue("CpusAvailable", int.Parse(line.Split("=")[1]));
                    break;
                }
            }

            output = GetBatchOutput("wmic computersystem get TotalPhysicalMemory /value");
            foreach (var line in output)
            {
                if (line.Contains("TotalPhysicalMemory"))
                {
                    var currentMemorySize = GetOptimizedSizeAndUnit(long.Parse(line.Split("=")[1]), out var unit);
                    collectedEventToTransform.SetFieldValue("MemorySize", currentMemorySize);
                    collectedEventToTransform.SetFieldValue("MemoryUnit", unit);
                    break;
                }
            }

            return collectedEventToTransform;
        }
        
        
    }
}