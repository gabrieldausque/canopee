using CanopeeElectronizedAgent.Datas;
using CanopeeElectronizedAgent.Models;
using Microsoft.AspNetCore.Mvc;

namespace CanopeeElectronizedAgent.Controllers
{
    public class LogsController : Controller
    {
        private ILogRepository _repository;

        public LogsController(ILogRepository repository)
        {
            _repository = repository;
        }
        public IActionResult GetAllLogs()
        {
            var logModel = new LogsViewModel()
            {
                Logs = _repository.GetLogs()
            }; 
            return View(logModel);
        }
    }
}