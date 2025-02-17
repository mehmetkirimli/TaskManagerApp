using Microsoft.AspNetCore.Mvc;

namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskDataController : ControllerBase
    {
        
        [HttpGet(Name = "GetTaskData")]
        public string Get()
        {
            return " Controller çalıştı :) ";
        }
    }
}
