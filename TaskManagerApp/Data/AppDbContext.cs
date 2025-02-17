using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Entity;

namespace TaskManagerApp.Data
{

    /*
     * IdentityDbContext<ApplicationUser>: Normal DbContext yerine
     * IdentityDbContext kullanıyoruz ki Identity tabloları otomatik olarak oluşsun
     */

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskData> TaskData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Identity tablosu oluşsun diye gerekli bu

            //TaskData ve ApplicationUser arasında 1-N ilişki
            builder.Entity<TaskData>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse TaskData ları da silinsin
        }



    }
}
