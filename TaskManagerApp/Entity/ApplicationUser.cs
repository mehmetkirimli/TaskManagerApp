using Microsoft.AspNetCore.Identity;

namespace TaskManagerApp.Entity
{
    public class ApplicationUser : IdentityUser
    {
        //Note : IdentityUser var olan alanlar 
        //Id,
        //UserName,
        //PasswordHash,
        //Email,
        //PhoneNumber,
        //EmailConfirmed,
        //PhoneNumberConfirmed,
        //TwoFactorEnabled,
        //LockoutEnd,
        //LockoutEnabled,
        //AccessFailedCount

        public ICollection<TaskData> Tasks { get; set; } = new List<TaskData>();

    }
}
