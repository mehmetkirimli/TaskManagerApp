using Microsoft.AspNetCore.Identity;
using TaskManagerApp.DTO;
using TaskManagerApp.Entity;

namespace TaskManagerApp.Service.Impl
{
    public interface ITaskDataService
    {
        Task<IEnumerable<TaskDataDto>> GetAllTasksAsync();
        Task<TaskDataDto> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskDataDto taskDto);
        Task UpdateTaskAsync(int id, TaskDataDto taskDto);
        Task DeleteTaskAsync(int id);
    }
}
