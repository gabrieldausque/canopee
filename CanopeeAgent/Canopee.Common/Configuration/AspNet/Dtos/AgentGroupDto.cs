namespace Canopee.Common.Configuration.AspNet.Dtos
{    
    /// <summary>
    /// Indicate a group that a specific agent belong to
    /// </summary>
    public class AgentGroupDto
    {
        /// <summary>
        /// The Agent Id
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// The group name
        /// </summary>
        public string Group { get; set; }
        
        /// <summary>
        /// The precedence order of the group for the agent
        /// </summary>
        public long Priority { get; set; }
    }
}