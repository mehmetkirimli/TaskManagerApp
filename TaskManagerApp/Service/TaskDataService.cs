using AutoMapper;
using TaskManagerApp.DTO;
using TaskManagerApp.Entity;
using TaskManagerApp.Repository.Impl;
using TaskManagerApp.Service.Impl;

namespace TaskManagerApp.Service
{
    public class TaskDataService : ITaskDataService
    {
        private readonly IRepository<TaskData> _taskRepo;
        private readonly IMapper _mapper;

        public TaskDataService(IRepository<TaskData> taskRepo, IMapper mapper)
        {
            _taskRepo = taskRepo;
            _mapper = mapper;
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
            var task = _mapper.Map<TaskData>(taskDto);
            task.UserId = userId; // task oluşturulurken kullanıcı id'si set ediliyor
            await _taskRepo.AddAsync(task);
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



    }
}
