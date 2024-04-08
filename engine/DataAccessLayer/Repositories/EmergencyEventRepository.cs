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

    public EmergencyEventRepository(EngineServiceContext context, ILogger logger)
    {
        _context = context;
    }

    public async Task<EmergencyEvent> CreateEmergencyEventAsync(EmergencyEvent emergencyEvent)
    {
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
}