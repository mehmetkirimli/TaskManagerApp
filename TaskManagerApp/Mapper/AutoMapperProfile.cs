using AutoMapper;
using TaskManagerApp.DTO;
using TaskManagerApp.Entity;

namespace TaskManagerApp.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TaskData, TaskDataDto>().ReverseMap();
            CreateMap<CreateTaskDto, TaskData>();
        }
    }
}
