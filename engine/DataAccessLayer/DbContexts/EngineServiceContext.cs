using BusinessLayer.Helpers;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataAccessLayer.DbContexts;

public class EngineServiceContext : IdentityDbContext<EmergencyAppUser>
{
    public EngineServiceContext(DbContextOptions<EngineServiceContext> options) : base(options)
    {
    }
    
    public DbSet<EmergencyEvent> EmergencyEvents { get; set; }
    public DbSet<Participant> Participants { get; set; }
    
    public DbSet<LandCoverAnalysis> LandCoverAnalyses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var roles = RoleConstants.Roles.Select(roleName => new IdentityRole{ Name = roleName, NormalizedName = roleName.ToUpper()}).ToList();

        modelBuilder.Entity<IdentityRole>().HasData(roles);
        
        modelBuilder.Entity<EmergencyEvent>()
            .HasOne(e => e.LandCoverAnalysis)
            .WithOne(l => l.EmergencyEvent)  
            .HasForeignKey<LandCoverAnalysis>(l => l.EmergencyEventId)
            .IsRequired(false);
    }
}