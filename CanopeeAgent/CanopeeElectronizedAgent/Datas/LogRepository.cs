using System.Collections.Generic;
using System.Linq;
using ElectronNET.API;

namespace CanopeeElectronizedAgent.Datas
{
    public class LogRepository : ILogRepository
    {
        
        private static object _lockObject = new object();
        private static LogRepository _instance;
        
        public static LogRepository Instance
        {
            get
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new LogRepository();
                    }
                }
                return _instance;
            }
        }
        
        private Queue<string> _buffer = new Queue<string>(500);

        public ICollection<string> GetLogs()
        {
            List<string> toReturn = new List<string>(); 
            lock (_lockObject)
            {
                toReturn.AddRange(_buffer.ToList());
            }

            return toReturn;
        }

        public void AddLog(string formattedLog)
        {
            lock (_lockObject)
            {
                if (_buffer.Count > 500)
                {
                    _buffer.Dequeue();
                }
                _buffer.Enqueue(formattedLog);
            }
        }
    }
}