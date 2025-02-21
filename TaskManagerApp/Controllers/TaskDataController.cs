using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTO;
using TaskManagerApp.DTO.Filter;
using TaskManagerApp.Entity;
using TaskManagerApp.Service;
using TaskManagerApp.Service.Impl;
using TaskManagerApp.Utils;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // JWT ile authorize olacak.
    [AuthorizeUser] // ActionFilterAttribute ile authorize olacak.

    /*
     * Mail'i Name olarak düşünerek , findByMail yoluyla ActionFilterAttribute ile authorize kontrolü yapıp yolluyoruz.
     */
    public class TaskController : ControllerBase
    {
        private readonly ITaskDataService _taskService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ITaskDataService taskService , UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.Id == null) return Unauthorized();
            return Ok(user);
        }

        // Get All Tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null) return Unauthorized();
            var tasks = await _taskService.GetAllTasksAsync(userId);
            return Ok(tasks);
        }

        // Get Task by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null) return Unauthorized();
            var task = await _taskService.GetTaskByIdAsync(id, userId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // Add New Task
        [HttpPost]
        public async Task<IActionResult> AddTask(TaskDataDto taskDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null) return Unauthorized();
            await _taskService.AddTaskAsync(taskDto,userId);
            return CreatedAtAction(nameof(GetTaskById), new { id = taskDto.Id }, taskDto);
        }

        // Update Task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskDataDto taskDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null) return Unauthorized();

            await _taskService.UpdateTaskAsync(id, taskDto,userId);
            return NoContent();
        }

        // Delete Task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null) return Unauthorized();

            await _taskService.DeleteTaskAsync(id,userId);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredTasks(TaskFilterDto filterDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null) return Unauthorized();
            var tasks = await _taskService.GetFilteredTasks(filterDto);
            return Ok(tasks);
        }
    }
}
