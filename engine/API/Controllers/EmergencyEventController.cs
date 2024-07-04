using API.Responses;
using BusinessLayer.DTOs.EmergencyEvent;
using BusinessLayer.Interfaces;
using DataAccessLayer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/emergency-events")]
public class EmergencyEventController : ControllerBase
{
    private readonly IEmergencyEventService _emergencyEventService;

    public EmergencyEventController(IEmergencyEventService emergencyEventService)
    {
        _emergencyEventService = emergencyEventService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<EmergencyDetailsDto>> GetEmergencyEventAsync([FromRoute] Guid id)
    {
        var result = await _emergencyEventService.GetEmergencyEventByIdAsync(id);
        if (!result.IsSuccess) return NotFound(EmergencyEventResponse.Failure(result.Errors));
        var usernames = await _emergencyEventService.GetParticipantUsernamesAsync(id);
        var authorUsername = await _emergencyEventService.GetAuthorUsernameAsync(result.Data.ReportedBy.ToString());
        var details = new EmergencyDetailsDto()
        {
            Id = result.Data.Id,
            Description = result.Data.Description,
            Location = result.Data.Location,
            Latitude = result.Data.Latitude,
            Longitude = result.Data.Longitude,
            Severity = result.Data.Severity,
            Status = result.Data.Status,
            Type = result.Data.Type,
            ReportedBy = result.Data.ReportedBy,
            ReportedByUsername = authorUsername.Data ?? "Anonymous",
            ReportedAt = result.Data.ReportedAt,
            UpdatedAt = result.Data.UpdatedAt,
            ParticipantsUsernames = usernames.Data,
            ParticipantsCount = usernames.Data.Count()
        };
        return Ok(EmergencyDetailsResponse.Success(details));
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmergencyEventDto>>> GetEmergencyEventsAsync([FromQuery] GetEventsParameters parameters)
    {
        var result = await _emergencyEventService.GetEmergencyEventsAsync(parameters);
        if (!result.IsSuccess) return NotFound(EmergencyEventResponse.Failure(result.Errors));
        return Ok(result.Data);
    }
    
    
    [Authorize]
    [HttpGet("participated")]
    public async Task<ActionResult<IEnumerable<EmergencyEventDto>>> GetParticipatedEventsAsync()
    {
        var userIdString = HttpContext.Items["userId"] as string;
        var result = await _emergencyEventService.GetParticipatedEventsAsync(userIdString);
        if (!result.IsSuccess) return NotFound(EmergencyEventResponse.Failure(result.Errors));
        return Ok(result.Data);
    }
    
    [Authorize]
    [HttpGet("markers")]
    public async Task<ActionResult<EmergencyEventMarkerResponse>> GetEmergencyEventMarkersAsync()
    {
        var result = await _emergencyEventService.GetEmergencyEventMarkersAsync();
        if (!result.IsSuccess) return NotFound(EmergencyEventMarkerResponse.Failure(result.Errors));
        return Ok(EmergencyEventMarkerResponse.Success(result.Data));
    } 
    
    [HttpPost]
    public async Task<ActionResult<EmergencyEventResponse>> CreateEmergencyEventAsync([FromBody] EmergencyEventCreationDto emergencyEventCreationDto)
    {
        try
        {
            var userIdString = HttpContext.Items["userId"] as string;
            var username = HttpContext.Items["username"] as string;
            var result = await _emergencyEventService.CreateEmergencyEventAsync(emergencyEventCreationDto, userIdString);
            if (!result.IsSuccess) return BadRequest(EmergencyEventResponse.Failure(result.Errors));
            var emergencyEvent = result.Data;
            await _emergencyEventService.AddParticipantAsync(emergencyEvent.Id, userIdString);
            await _emergencyEventService.CreateEmptyAnalysisAsync(emergencyEvent.Id);
            await _emergencyEventService.PublishEmergencyReportedAsync(emergencyEvent, username);
            return Created($"api/emergency-events/{emergencyEvent.Id}", EmergencyEventResponse.Success(emergencyEvent));
        }
        catch (Exception ex)
        {
            return BadRequest(EmergencyEventResponse.Failure(new List<string>(){ex.Message}));
        }
    }
    
    [HttpPost("{emergencyEventId}/participants")]
    public async Task<ActionResult<BaseResponse>> AddParticipantAsync([FromRoute] Guid emergencyEventId)
    {
        var userId = HttpContext.Items["userId"] as string;
        var result = await _emergencyEventService.AddParticipantAsync(emergencyEventId, userId);
        if (!result.IsSuccess) return BadRequest(BaseResponse.Failure(result.Errors));
        return Ok(BaseResponse.Success());
    }
}