using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContexts;

public class EngineServiceContext : IdentityDbContext<EmergencyAppUser>
{
    public EngineServiceContext(DbContextOptions<EngineServiceContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var roles = RoleConstants.Roles.Select(roleName => new IdentityRole{ Name = roleName, NormalizedName = roleName.ToUpper()}).ToList();

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}