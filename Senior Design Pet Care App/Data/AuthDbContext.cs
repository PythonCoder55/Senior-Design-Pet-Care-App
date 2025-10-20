using Senior_Design_Pet_Care_App.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Senior_Design_Pet_Care_App.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = 1,
                Email = "admin@gmail.com",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            // Hash the password "admin"
            admin.PasswordHash = hasher.HashPassword(admin, "adminPass1!");

            modelBuilder.Entity<User>().HasData(admin);
        }
    }
}