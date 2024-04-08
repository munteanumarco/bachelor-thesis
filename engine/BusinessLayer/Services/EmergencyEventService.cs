using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.DTOs.EmergencyEvent;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Serilog;

namespace BusinessLayer.Services;

public class EmergencyEventService : IEmergencyEventService
{
    private readonly IEmergencyEventRepository _emergencyEventRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public EmergencyEventService(IEmergencyEventRepository emergencyEventRepository, IMapper mapper, ILogger logger)
    {
        _emergencyEventRepository = emergencyEventRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OperationResult<EmergencyEventDto>> CreateEmergencyEventAsync(EmergencyEventCreationDto emergencyEventCreationDto)
    {
        try
        {
            var emergencyEvent = _mapper.Map<EmergencyEvent>(emergencyEventCreationDto);
            var createdEvent = _mapper.Map<EmergencyEventDto>(await _emergencyEventRepository.CreateEmergencyEventAsync(emergencyEvent));
            return OperationResult<EmergencyEventDto>.Success(createdEvent);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating an emergency event.");
            return OperationResult<EmergencyEventDto>.Failure(new List<string>(){ex.Message});
        }
    }
    
    public async Task<OperationResult<EmergencyEventDto>> GetEmergencyEventByIdAsync(Guid id)
    {
        var emergencyEvent = await _emergencyEventRepository.GetEmergencyEventByIdAsync(id);
        if (emergencyEvent == null) return OperationResult<EmergencyEventDto>.Failure(new List<string>(){"Emergency event not found."});
        return OperationResult<EmergencyEventDto>.Success(_mapper.Map<EmergencyEventDto>(emergencyEvent));
    }
    
    public async Task<OperationResult<PagedResultDto<EmergencyEventDto>>> GetEmergencyEventsAsync(GetEventsParameters parameters)
    {
        var paginatedResult = await _emergencyEventRepository.GetEmergencyEventsAsync(parameters);
        return OperationResult<PagedResultDto<EmergencyEventDto>>.Success(_mapper.Map<PagedResultDto<EmergencyEventDto>>(paginatedResult));
    }
}