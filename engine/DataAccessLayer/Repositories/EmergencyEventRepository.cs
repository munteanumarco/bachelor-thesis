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
        IQueryable<EmergencyEvent> query = _context.EmergencyEvents;
        if (parameters.UserId != null)
        {
            query = query.Where(emergencyEvent => emergencyEvent.ReportedBy == Guid.Parse(parameters.UserId));
            var count = await query.CountAsync();
            return new PagedResult<EmergencyEvent>(await query.ToListAsync(), count, parameters.PageNumber, parameters.PageSize);
        }
        else
        {
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResult<EmergencyEvent>(items, count, parameters.PageNumber, parameters.PageSize);
        }
    }
    
    public async Task<IEnumerable<Guid>> GetParticipatedEventIdsAsync(string userId)
    {
        return await _context.Participants
            .Where(participant => participant.UserId == userId)
            .Select(participant => participant.EmergencyEventId)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmergencyEvent>> GetEmergencyEventsForIds(IEnumerable<Guid> eventIds)
    {
        var events = await _context.EmergencyEvents
            .Where(emergencyEvent => eventIds.Contains(emergencyEvent.Id))
            .ToListAsync();

        return events;
    }

    public async Task<bool> CreateLandCoverAnalysisAsync(LandCoverAnalysis landCoverAnalysis) 
    {
        await _context.LandCoverAnalyses.AddAsync(landCoverAnalysis);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> GetAuthorUsernameAsync(string userId)
    {
        return await _context.Users
            .Where(user => user.Id == userId)
            .Select(user => user.UserName)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<string>> GetParticipantUsernamesAsync(Guid emergencyEventId)
    {
        return await _context.Participants
            .Where(participant => participant.EmergencyEventId == emergencyEventId)
            .Include(participant => participant.User)
            .Select(participant => participant.User.UserName)
            .ToListAsync();
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