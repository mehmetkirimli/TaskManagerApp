using Microsoft.AspNetCore.Identity;
using TaskManagerApp.DTO;
using TaskManagerApp.DTO.Filter;
using TaskManagerApp.Entity;

namespace TaskManagerApp.Service.Impl
{
    public interface ITaskDataService
    {
        Task<IEnumerable<TaskDataDto>> GetAllTasksAsync(string userId);
        Task<TaskDataDto> GetTaskByIdAsync(int id, string userId);
        Task AddTaskAsync(TaskDataDto taskDto , string userId);
        Task UpdateTaskAsync(int id, TaskDataDto taskDto , string userId);
        Task DeleteTaskAsync(int id , string userId);
        Task<List<TaskDataDto>> GetFilteredTasks(TaskFilterDto filterDto);
    }
}
