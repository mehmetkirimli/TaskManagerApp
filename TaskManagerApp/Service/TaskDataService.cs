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

        public async Task<IEnumerable<TaskDataDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskDataDto>>(tasks);
        }

        public async Task<TaskDataDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            return _mapper.Map<TaskDataDto>(task);
        }

        public async Task AddTaskAsync(TaskDataDto taskDto)
        {
            var task = _mapper.Map<TaskData>(taskDto);
            await _taskRepo.AddAsync(task);
        }

        public async Task UpdateTaskAsync(int id, TaskDataDto taskDto)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) return;

            _mapper.Map(taskDto, task);
            await _taskRepo.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) return;

            await _taskRepo.DeleteAsync(id);
        }



    }
}
