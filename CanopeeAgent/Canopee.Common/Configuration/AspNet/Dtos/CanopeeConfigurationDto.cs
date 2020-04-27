namespace Canopee.Common.Configuration.AspNet.Dtos
{
    /// <summary>
    /// A configuration associated to a agent and/or a group. 
    /// </summary>
    public class CanopeeConfigurationDto
    {
        /// <summary>
        /// The configuration as Json
        /// </summary>
        public JsonObject Configuration { get; set; }
        /// <summary>
        /// The agent Id. If set to "Default", it concerns all agent
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// The group name. If set to "Default", it concerns all group (or no group) 
        /// </summary>
        public string Group { get; set; }

    }
}