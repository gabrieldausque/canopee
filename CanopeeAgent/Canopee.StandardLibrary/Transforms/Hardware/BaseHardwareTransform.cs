using System.Collections.Generic;
using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Transforms.Hardware
{
    /// <summary>
    /// Base class that will add hardware information in a <see cref="ICollectedEvent"/>
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
    ///                             "TransformType": "Hardware",
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
    /// The TransformType will be Hardware
    /// The OSSpecific will be set to true as hardware infos are obtained in different way specific to OS
    /// 
    /// </summary>
    public abstract class BaseHardwareTransform : BatchTransform
    {
        /// <summary>
        /// Default constructor, Initiliaze the Units Repository, used for conversion in human readable format
        /// </summary>
        public BaseHardwareTransform()
        {
            UnitsRepository = new Dictionary<string, string>()
            {
                {"T","Tb" },
                {"Ti","Tb" },
                {"G", "Gb"},
                {"Gi", "Gb"},
                {"M", "Mb"},
                {"Mi", "Mb"}
            };

        }

        /// <summary>
        /// Get human readable format from a bytes value.
        /// </summary>
        /// <param name="originalSize">a quantity in bytes</param>
        /// <param name="unit">The human readable unit</param>
        /// <returns>the quantity converted in the unit</returns>
        protected float GetOptimizedSizeAndUnit(float originalSize, out string unit)
        {
            if (originalSize > 1000000000000f)
            {
                unit = "Tb";
                return originalSize / 1000000000000f;                
            }
            else if (originalSize > 1000000000f)
            {
                unit = "Gb";
                return originalSize / 1000000000f;
            }
            else if (originalSize > 1000000f)
            {
                unit = "Mb";
                return originalSize / 1000000f;
            }
            else if (originalSize > 1000f)
            {
                unit = "Kb";
                return originalSize / 1000f;
            }
            else
            {
                unit = "b";
                return originalSize;
            };
        }

        /// <summary>
        /// A mapping to normalize a customUnit label in a standard unit label
        /// </summary>
        public Dictionary<string,string> UnitsRepository { get; set; }
        
        /// <summary>
        /// Convert a mapped custom unit label in a standard unit label
        /// </summary>
        /// <param name="customUnit"></param>
        /// <returns></returns>
        protected string GetSizeUnit(string customUnit)
        {
            if (UnitsRepository.ContainsKey(customUnit))
                return UnitsRepository[customUnit];
            return customUnit;
        }

    }
}