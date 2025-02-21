namespace TaskManagerApp.DTO.Filter
{
    public class TaskFilterDto
    {
        public string userId { get; set; }
        public string Title { get; set; }
        public bool? isCompleted { get; set; } // null , true, false
    }
}
