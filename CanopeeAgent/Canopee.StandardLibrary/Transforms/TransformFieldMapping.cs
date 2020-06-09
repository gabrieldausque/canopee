using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Transforms
{
    /// <summary>
    /// Describe a mapping between an external object and a <see cref="ICollectedEvent"/> field
    /// </summary>
    public class TransformFieldMapping
    {
        /// <summary>
        /// The name in the external object
        /// </summary>
        public string SearchedName { get; set; }
        
        /// <summary>
        /// The name in the <see cref="ICollectedEvent"/>
        /// </summary>
        public string LocalName { get; set; }
    }
}