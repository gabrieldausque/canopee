using System.Collections.Generic;

namespace Canopee.StandardLibrary.Transforms.Hardware
{
    public abstract class BaseHardwareTransform : BatchTransform
    {
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

        public Dictionary<string,string> UnitsRepository { get; set; }
        
        protected string GetSizeUnit(string customUnit)
        {
            if (UnitsRepository.ContainsKey(customUnit))
                return UnitsRepository[customUnit];
            return customUnit;
        }

    }
}