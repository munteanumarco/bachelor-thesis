using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContexts;

public class CommunicationServiceContext : IdentityDbContext<EmergencyAppUser>
{
    public CommunicationServiceContext(DbContextOptions<CommunicationServiceContext> options) 
        : base(options)
    {
    }
    
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatEvent?> ChatEvents { get; set; }
    public DbSet<Participant> Participants { get; set; }
    
    public DbSet<EmergencyEvent> EmergencyEvents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ChatEvent>()
            .HasOne(chatEvent => chatEvent.EmergencyEvent)
            .WithOne(evnt => evnt.ChatEvent)
            .HasForeignKey<ChatEvent>(chatEvent => chatEvent.EmergencyEventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatEvent>()
            .HasMany(chatEvent => chatEvent.ChatMessages)
            .WithOne(chatMessage => chatMessage.ChatEvent)
            .HasForeignKey(chatMessage => chatMessage.ChatId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(chatMessage => chatMessage.Message)
            .WithOne(message => message.ChatMessage)
            .HasForeignKey<ChatMessage>(chatMessage => chatMessage.MessageId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Participant>()
            .HasIndex(p => new { p.EmergencyEventId, p.UserId })
            .IsUnique();

        modelBuilder.Entity<Participant>()
            .HasOne(participant => participant.User)
            .WithMany(user => user.Participants)
            .HasForeignKey(participant => participant.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Participant>()
            .HasOne(participant => participant.Event)
            .WithMany(evnt => evnt.Participants)
            .HasForeignKey(participant => participant.EmergencyEventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}