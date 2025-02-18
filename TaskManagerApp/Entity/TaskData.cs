using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Entity
{
    public class TaskData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set;} = DateTime.Now;
        public DateTime? UpdatedDate { get; set;}
        public bool IsCompleted { get; set;} = false;

        [Required]
        public string UserId { get; set; } // String olarak tutulacak çünkü IdentityUser'dan gelen Id string olarak tanımlı

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
