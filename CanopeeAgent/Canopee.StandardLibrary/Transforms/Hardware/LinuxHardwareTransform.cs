using System.Composition;
using System.Text.RegularExpressions;
using Canopee.Common;

namespace Canopee.StandardLibrary.Transforms.Hardware
{
    [Export("HardwareLINUX",typeof(ITransform))]
    public class HardwareTransform : BatchTransform
    {
        public override ICollectedEvent Transform(ICollectedEvent input)
        {
            var lines = GetBatchOutput("lscpu");
            var regex = new Regex(".+:[ ]+(.+)");
            var matches = regex.Match(lines[0]);
            input.SetFieldValue("CpuArchitecture", matches.Groups[1].Value);
            matches = regex.Match(lines[4]);
            input.SetFieldValue("CpusAvailable", int.Parse(matches.Groups[1].Value));
            matches = regex.Match(lines[13]);
            input.SetFieldValue("CpuModel",matches.Groups[1].Value);
            lines = GetBatchOutput("\"free -h\"");
            regex = new Regex(".+:[ ]+(?<size>[0-9\\.,]+)(?<unit>[a-zA-Z]+)[ ]+.*");
            matches = regex.Match(lines[1]);
            input.SetFieldValue("MemorySize", float.Parse(matches.Groups["size"].Value));
            infos.MemoryUnit = GetSizeUnit(matches.Groups["unit"].Value);
        }
    }
}