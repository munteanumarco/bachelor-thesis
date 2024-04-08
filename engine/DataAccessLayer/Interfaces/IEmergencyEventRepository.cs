using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;

namespace DataAccessLayer.Interfaces;

public interface IEmergencyEventRepository
{
    Task<EmergencyEvent> CreateEmergencyEventAsync(EmergencyEvent emergencyEvent);
    Task<EmergencyEvent?> GetEmergencyEventByIdAsync(Guid id);
    Task<PagedResult<EmergencyEvent>> GetEmergencyEventsAsync(GetEventsParameters parameters);
}