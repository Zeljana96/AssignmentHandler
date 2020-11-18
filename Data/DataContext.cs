using Microsoft.EntityFrameworkCore;
using Tasks_Handler.Models;

namespace Tasks_Handler.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Property(user => user.Role).HasDefaultValue("User");
        }
    }
}