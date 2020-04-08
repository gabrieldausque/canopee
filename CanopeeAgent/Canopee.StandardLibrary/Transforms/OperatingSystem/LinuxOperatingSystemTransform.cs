using System;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
{
    [Export("OperatingSystemLINUX", typeof(ITransform))]
    public class LinuxOperatingSystemTransform : BatchTransform
    {
        public override ICollectedEvent Transform(ICollectedEvent input)
        {
            input.SetFieldValue("OperatingSystem", GetBatchOutput("\"uname -o\"")[0]);
            input.SetFieldValue("OperatingSystemVersion", GetBatchOutput("\"uname -v\"")[0]);
            return input;
        }
    }
}