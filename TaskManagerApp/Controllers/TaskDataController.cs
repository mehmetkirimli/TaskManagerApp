using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTO;
using TaskManagerApp.Service;
using TaskManagerApp.Service.Impl;
using TaskManagerApp.Utils;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // JWT ile authorize olacak.
    [AuthorizeUser] // ActionFilterAttribute ile authorize olacak.
    public class TaskController : ControllerBase
    {
        private readonly ITaskDataService _taskService;

        public TaskController(ITaskDataService taskService)
        {
            _taskService = taskService;
        }

        // Get All Tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var userId = User.Identity?.Name;
            if (userId == null) return Unauthorized();
            var tasks = await _taskService.GetAllTasksAsync(userId);
            return Ok(tasks);
        }

        // Get Task by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var userId = User.Identity?.Name;
            var task = await _taskService.GetTaskByIdAsync(id, userId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // Add New Task
        [HttpPost]
        public async Task<IActionResult> AddTask(TaskDataDto taskDto)
        {
            var userId = User.Identity?.Name;
            await _taskService.AddTaskAsync(taskDto,userId);
            return CreatedAtAction(nameof(GetTaskById), new { id = taskDto.Id }, taskDto);
        }

        // Update Task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskDataDto taskDto)
        {
            var userId = User.Identity?.Name;
            await _taskService.UpdateTaskAsync(id, taskDto,userId);
            return NoContent();
        }

        // Delete Task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = User.Identity?.Name;
            await _taskService.DeleteTaskAsync(id,userId);
            return NoContent();
        }
    }
}
