using Microsoft.AspNetCore.Mvc;

namespace CanopeeElectronizedAgent.Controllers
{
    public class LogsController : Controller
    {
        public IActionResult GetAllLogs()
        {
            return View();
        }
    }
}