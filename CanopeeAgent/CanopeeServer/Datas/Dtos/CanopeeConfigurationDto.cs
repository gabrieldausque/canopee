using Canopee.Common;

namespace CanopeeServer.Datas.Dtos
{
    public class CanopeeConfigurationDto
    {
        public JsonObject Configuration { get; set; }

        public string AgentId { get; set; }
        
        public string Group { get; set; }
        public int Priority { get; set; }
    }
}