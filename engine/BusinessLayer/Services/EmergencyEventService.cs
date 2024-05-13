using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.DTOs.EmergencyEvent;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.EventContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using MassTransit;
using Serilog;

namespace BusinessLayer.Services;

public class EmergencyEventService : IEmergencyEventService
{
    private readonly IEmergencyEventRepository _emergencyEventRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public EmergencyEventService(IEmergencyEventRepository emergencyEventRepository, IMapper mapper, ILogger logger, IPublishEndpoint publishEndpoint)
    {
        _emergencyEventRepository = emergencyEventRepository;
        _mapper = mapper;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<OperationResult<EmergencyEventDto>> CreateEmergencyEventAsync(EmergencyEventCreationDto emergencyEventCreationDto, string? userIdString)
    {
        try
        {
            var emergencyEvent = _mapper.Map<EmergencyEvent>(emergencyEventCreationDto);
            if (userIdString != null) emergencyEvent.ReportedBy = Guid.Parse(userIdString);
            emergencyEvent.Status = Status.New;
            emergencyEvent.ReportedAt = DateTime.Now;
            emergencyEvent.UpdatedAt = DateTime.Now;
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
    
    public async Task<OperationResult<IEnumerable<EmergencyEventMarkerDto>>> GetEmergencyEventMarkersAsync()
    {
        var emergencyEvents = await _emergencyEventRepository.GetEmergencyEventMarkersAsync();
        return OperationResult<IEnumerable<EmergencyEventMarkerDto>>.Success(_mapper.Map<IEnumerable<EmergencyEventMarkerDto>>(emergencyEvents));
    }

    public async Task<OperationResult<bool>> AddParticipantAsync(Guid emergencyEventId, string userId)
    {
        var isParticipant = await _emergencyEventRepository.IsParticipantAsync(emergencyEventId, userId);
        if (isParticipant) return OperationResult<bool>.Failure(new List<string>(){"You are already a participant."});
        var result = await _emergencyEventRepository.AddParticipantAsync(emergencyEventId, userId);
        if (!result) return OperationResult<bool>.Failure(new List<string>(){"Failed to add participant."});
        return OperationResult<bool>.Success(true);
    }

    public async Task PublishEmergencyReportedAsync(EmergencyEventDto emergencyEventDto, string? username)
    {
        try
        {
            var emergencyEventEntity = await _emergencyEventRepository.GetEmergencyEventByIdAsync(emergencyEventDto.Id);
            if (emergencyEventEntity == null) throw new Exception("Event not found");
            
            await _publishEndpoint.Publish(new EmergencyReportedEvent()
            {
                Id = emergencyEventEntity.Id,
                Description = emergencyEventEntity.Description,
                Location = emergencyEventEntity.Location,
                Latitude = emergencyEventEntity.Latitude,
                Longitude = emergencyEventEntity.Longitude,
                Type = emergencyEventEntity.Type,
                Severity = emergencyEventEntity.Severity,
                Status = emergencyEventEntity.Status,
                ReportedBy = emergencyEventEntity.ReportedBy ?? Guid.Empty,
                ReportedByUsername = username ?? "Anonymous",
                ReportedAt = emergencyEventEntity.ReportedAt,
                UpdatedAt = emergencyEventEntity.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while publishing user created event");
            throw;
        }
    }
}