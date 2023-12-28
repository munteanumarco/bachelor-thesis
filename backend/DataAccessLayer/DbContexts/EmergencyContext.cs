using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContexts;

public class EmergencyContext : DbContext
{
    public EmergencyContext(DbContextOptions<EmergencyContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "email@email.com",
                Password = "admin"
            });
    }
}