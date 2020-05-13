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
    /// <summary>
    /// <see cref="ITransform"/> that will add OS information in a <see cref="ICollectedEvent"/> for Linux
    ///
    /// use uname command
    /// 
    /// Configuration will be :
    /// <example>
    /// <code>
    ///
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Transforms" : [
    ///                         {
    ///                             "TransformType": "OperatingSystem",
    ///                             "OSSpecific": true
    ///                        }
    ///                     ]
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
    /// The TransformType will be OperatingSystem
    /// The OSSpecific will be set to true as OperatingSystem infos are obtained in different way specific to OS
    /// 
    /// </summary>
    [Export("OperatingSystemLINUX", typeof(ITransform))]
    public class LinuxOperatingSystemTransform : BatchTransform
    {
        /// <summary>
        /// Add Operating System information to the <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="collectedEventToTransform">The <see cref="ICollectedEvent"/> to transform</param>
        /// <returns>the transformed <see cref="ICollectedEvent"/></returns>
        public override ICollectedEvent Transform(ICollectedEvent collectedEventToTransform)
        {
            collectedEventToTransform.SetFieldValue("OperatingSystem", GetBatchOutput("\"uname -o\"")[0]);
            collectedEventToTransform.SetFieldValue("OperatingSystemVersion", GetBatchOutput("\"uname -v\"")[0]);
            return collectedEventToTransform;
        }
    }
}