namespace TaskManagerApp.DTO
{
    public class TaskDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        //CreatedDate , UpdatedDate ve UserId gerek yok
    }
}
