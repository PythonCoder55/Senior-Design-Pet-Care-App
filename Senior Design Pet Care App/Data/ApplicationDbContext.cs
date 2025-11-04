using Senior_Design_Pet_Care_App.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Senior_Design_Pet_Care_App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Reminder> Reminders { get; set; } = null!;
        public DbSet<Pet> Pets { get; set; } = null!; // <<--- new

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //var hasher = new PasswordHasher<User>();

            //var admin = new User
            //{
            //    Id = 1,
            //    Email = "admin@gmail.com",
            //    Role = "Admin",
            //    CreatedAt = DateTime.UtcNow
            //};

            //// Hash the password "adminPass1!"
            //admin.PasswordHash = hasher.HashPassword(admin, "adminPass1!");

            //modelBuilder.Entity<User>().HasData(admin);

            // store enum as string (optional) so DB is readable
            modelBuilder.Entity<Reminder>()
                .Property(r => r.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Pet>()
                .Property(p => p.ActivityLevel)
                .HasConversion<string>();
        }
    }
}
