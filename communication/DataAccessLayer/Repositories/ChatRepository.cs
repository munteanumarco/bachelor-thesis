using DataAccessLayer.DbContexts;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly CommunicationServiceContext _context;

    public ChatRepository(CommunicationServiceContext context)
    {
        _context = context;
    }

    public async Task<Message?> SaveMessageAsync(Message newMessage)
    {
        await _context.Messages.AddAsync(newMessage);
        await _context.SaveChangesAsync();
        
        var messageWithUser = await _context.Messages
            .Include(message => message.User)
            .Include(message => message.ChatMessage)
            .FirstOrDefaultAsync(m => m.Id == newMessage.Id);
        
        return messageWithUser;
    }

    public async Task<IEnumerable<Guid>> GetUserChatIds(string userId)
    {
        var result = await _context.Participants
            .Where(participant => participant.UserId == userId)
            .Include(participant => participant.Event)
            .ThenInclude(evnt => evnt.ChatEvent)
            .Select(participant => participant.Event.ChatEvent.Id)
            .ToListAsync();
        return result;
    }
    
    public async Task<string> SaveChatMessageAsync(ChatMessage newChatMessage)
    {
        await _context.ChatMessages.AddAsync(newChatMessage);
        await _context.SaveChangesAsync();
        return "Chat message saved";
    }
    
    public async Task<IEnumerable<Message>> GetChatMessagesAsync(Guid chatId)
    {
        var query = _context.ChatEvents
            .Where(chatEvent => chatEvent.Id == chatId)
            .Include(chatEvent => chatEvent.ChatMessages)
            .ThenInclude(chatMessage => chatMessage.Message)
            .ThenInclude(message => message.User)
            .SelectMany(chatEvent => chatEvent.ChatMessages)
            .Select(chatMessage => chatMessage.Message)
            .OrderBy(message => message.Date);

        return await query.ToListAsync();
    }
    
    public async Task AddChatEventAsync(Guid emergencyEventId, string name)
    {
        var chatEvent = new ChatEvent()
        {
            EmergencyEventId = emergencyEventId,
            Name = name
        };
        await _context.ChatEvents.AddAsync(chatEvent);
        await _context.SaveChangesAsync();
    }

    public async Task<ChatEvent?> GetChatDetailsAsync(Guid eventId)
    {
        return await _context.ChatEvents
            .Where(chatEvent => chatEvent.EmergencyEventId == eventId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetParticipantCountAsync(Guid eventId)
    {
        return await _context.Participants
            .Where(participant => participant.EmergencyEventId == eventId)
            .CountAsync();
    }

    public Task<IEnumerable<Participant>> GetAdministratorsAsync()
    {
        throw new NotImplementedException();
    }
}