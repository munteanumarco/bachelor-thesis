using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContexts;

public class CommunicationServiceContext : DbContext
{
    public CommunicationServiceContext(DbContextOptions<CommunicationServiceContext> options) 
        : base(options)
    {
    }
    
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatEvent> ChatEvents { get; set; }
    
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
    }
}