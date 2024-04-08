using BusinessLayer.DTOs;
using BusinessLayer.DTOs.EmergencyEvent;
using BusinessLayer.Helpers;
using DataAccessLayer.Helpers;

namespace BusinessLayer.Interfaces;

public interface IEmergencyEventService
{
    Task<OperationResult<EmergencyEventDto>> CreateEmergencyEventAsync(
        EmergencyEventCreationDto emergencyEventCreationDto);
    Task<OperationResult<EmergencyEventDto>> GetEmergencyEventByIdAsync(Guid id);
    Task<OperationResult<PagedResultDto<EmergencyEventDto>>> GetEmergencyEventsAsync(GetEventsParameters parameters);
}