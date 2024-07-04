using BusinessLayer.DTOs;
using BusinessLayer.DTOs.EmergencyEvent;
using BusinessLayer.Helpers;
using DataAccessLayer.Helpers;

namespace BusinessLayer.Interfaces;

public interface IEmergencyEventService
{
    Task<OperationResult<EmergencyEventDto>> CreateEmergencyEventAsync(
        EmergencyEventCreationDto emergencyEventCreationDto, string? userIdString);
    Task<OperationResult<EmergencyEventDto>> GetEmergencyEventByIdAsync(Guid id);
    Task<OperationResult<PagedResultDto<EmergencyEventDto>>> GetEmergencyEventsAsync(GetEventsParameters parameters);
    Task<OperationResult<IEnumerable<EmergencyEventMarkerDto>>> GetEmergencyEventMarkersAsync();
    Task<OperationResult<IEnumerable<EmergencyEventDto>>> GetParticipatedEventsAsync(string userId);
    Task<OperationResult<bool>> AddParticipantAsync(Guid emergencyEventId, string userId);
    Task<OperationResult<IEnumerable<string>>> GetParticipantUsernamesAsync(Guid emergencyEventId);
    Task<OperationResult<string>> GetAuthorUsernameAsync(string userId);
    Task<OperationResult<bool>> CreateEmptyAnalysisAsync(Guid emergencyEventId);
    Task PublishEmergencyReportedAsync(EmergencyEventDto emergencyEventDto, string? username);
}