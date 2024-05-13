using System.ComponentModel;
using DataAccessLayer.DbContexts;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataAccessLayer.Repositories;

public class EmergencyEventRepository : IEmergencyEventRepository
{
    private readonly EngineServiceContext _context;
    private readonly Serilog.ILogger _logger;

    public EmergencyEventRepository(EngineServiceContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EmergencyEvent> CreateEmergencyEventAsync(EmergencyEvent emergencyEvent)
    {
        emergencyEvent.ReportedAt = emergencyEvent.ReportedAt.ToUniversalTime();
        emergencyEvent.UpdatedAt = emergencyEvent.UpdatedAt.ToUniversalTime();
        await _context.EmergencyEvents.AddAsync(emergencyEvent);
        await _context.SaveChangesAsync();
        return emergencyEvent;
    }

    public async Task<EmergencyEvent?> GetEmergencyEventByIdAsync(Guid id)
    {
        return await _context.EmergencyEvents.FindAsync(id);
    }
    
    public async Task<PagedResult<EmergencyEvent>> GetEmergencyEventsAsync(GetEventsParameters parameters)
    {
        var items = await _context.EmergencyEvents
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();
        var count = await _context.EmergencyEvents.CountAsync();
        return new PagedResult<EmergencyEvent>(items, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<IEnumerable<EmergencyEvent>> GetEmergencyEventMarkersAsync()
    {
        return await _context.EmergencyEvents
            .Where(e => e.Status != Status.Resolved)
            .ToListAsync();
    }

    public async Task<bool> AddParticipantAsync(Guid emergencyEventId, string userId)
    {
        try
        {
            await _context.Participants.AddAsync(new Participant
            {
                EmergencyEventId = emergencyEventId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while adding a participant to an emergency event.");
            return false;
        }

    }

    public async Task<bool> IsParticipantAsync(Guid emergencyEventId, string userId)
    {
        return await _context.Participants.AnyAsync(participant => participant.EmergencyEventId == emergencyEventId && participant.UserId == userId);
    }
}