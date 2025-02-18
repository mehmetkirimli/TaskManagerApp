using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTO;
using TaskManagerApp.Service;
using TaskManagerApp.Service.Impl;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // JWT ile authorize olacak.
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
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        // Get Task by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // Add New Task
        [HttpPost]
        public async Task<IActionResult> AddTask(TaskDataDto taskDto)
        {
            await _taskService.AddTaskAsync(taskDto);
            return CreatedAtAction(nameof(GetTaskById), new { id = taskDto.Id }, taskDto);
        }

        // Update Task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskDataDto taskDto)
        {
            await _taskService.UpdateTaskAsync(id, taskDto);
            return NoContent();
        }

        // Delete Task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
