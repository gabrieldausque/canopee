using System.Collections.Generic;

namespace CanopeeElectronizedAgent.Datas
{
    public interface ILogRepository
    {
        ICollection<string> GetLogs();

        void AddLog(string formattedLog);
    }
}