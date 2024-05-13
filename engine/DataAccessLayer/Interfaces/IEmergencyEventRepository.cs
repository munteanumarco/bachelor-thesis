using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;

namespace DataAccessLayer.Interfaces;

public interface IEmergencyEventRepository
{
    Task<EmergencyEvent> CreateEmergencyEventAsync(EmergencyEvent emergencyEvent);
    Task<EmergencyEvent?> GetEmergencyEventByIdAsync(Guid id);
    Task<PagedResult<EmergencyEvent>> GetEmergencyEventsAsync(GetEventsParameters parameters);
    Task<IEnumerable<EmergencyEvent>> GetEmergencyEventMarkersAsync();
    Task<bool> AddParticipantAsync(Guid emergencyEventId, string userId);
    Task<bool> IsParticipantAsync(Guid emergencyEventId, string userId);
}