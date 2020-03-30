using System;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Canopee.Common;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
{
    [Export("OperatingSystemWINDOWS", typeof(ITransform))]
    public class WindowsOperatingSystemTransform : BatchTransform
    {
        public override ICollectedEvent Transform(ICollectedEvent input)
        {
            foreach(var line in GetBatchOutput(@"wmic OS get Version,Name,OSArchitecture /value"))
            {
                if (line.Contains("Name"))
                {
                    input.SetFieldValue("OperatingSystem", line.Split('=')[1]);
                } 
                else if (line.Contains("Version"))
                {
                    input.SetFieldValue("OperatingSystemVersion", line.Split('=')[1]);
                }
            }
            return input;
        }
    }
}