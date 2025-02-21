using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TaskManagerApp.DTO;
using TaskManagerApp.DTO.Filter;
using TaskManagerApp.Entity;
using TaskManagerApp.Repository.Impl;
using TaskManagerApp.Service.Impl;
using TaskManagerApp.Utils;

namespace TaskManagerApp.Service
{
    public class TaskDataService : ITaskDataService
    {
        private readonly IRepository<TaskData> _taskRepo ;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskDataService(IRepository<TaskData> taskRepo, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _taskRepo = taskRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<TaskDataDto>> GetAllTasksAsync(string userId)
        {
            // kullanıcıya ait tasklar için sorguyu filtrelendiricez
            var tasks = await _taskRepo.GetAllAsync(t=>t.UserId == userId);
            return _mapper.Map<IEnumerable<TaskDataDto>>(tasks);
        }

        public async Task<TaskDataDto> GetTaskByIdAsync(int id, string userId)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bu görevi görüntülemek için yetkiniz yok.");
            }
            return _mapper.Map<TaskDataDto>(task);
        }

        public async Task AddTaskAsync(TaskDataDto taskDto , string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var taskData = new TaskData
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                UserId = userId // Burada kullanıcı kimliğini doğru şekilde bağlıyoruz
            };

            await _taskRepo.AddAsync(taskData);
        }

        public async Task UpdateTaskAsync(int id, TaskDataDto taskDto , string userId)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) 
            {
                throw new KeyNotFoundException("Görev Bulunamadı !!");
            }

            if(task.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bu görevi güncellemek için yetkiniz yok.");
            }
            task.UpdatedDate = DateTime.Now;
            _mapper.Map(taskDto, task);
            await _taskRepo.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id, string userId)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) 
            {
                throw new KeyNotFoundException("Görev Bulunamadı !!");
            }

            if (task.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bu görevi silmek için yetkiniz yok.");
            }

            await _taskRepo.DeleteAsync(id);
        }

        public async Task<List<TaskDataDto>> GetFilteredTasks(TaskFilterDto filterDto)
        {
            // Filtreleme ifadesini hazırlıyoruz
            var userId = filterDto.userId;

            Expression<Func<TaskData, bool>> filterExpression = t =>t.UserId == userId ;

            if (!string.IsNullOrEmpty(filterDto.Title))
            {
                filterExpression = filterExpression.AndAlso(t => t.Title.Contains(filterDto.Title));
            }

            if (filterDto.isCompleted.HasValue)
            {
                filterExpression = filterExpression.AndAlso(t => t.IsCompleted == filterDto.isCompleted.Value);
            }

            var tasks = await _taskRepo.GetFilteredAsync(filterExpression);
            return _mapper.Map<List<TaskDataDto>>(tasks);
        }

    }
}
