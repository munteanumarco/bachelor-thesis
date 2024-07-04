using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs.EmergencyEvent;
using API.Responses;
using BusinessLayer.Helpers;
using DataAccessLayer.Entities;

public class EmergencyEventControllerTests
{
    private readonly Mock<IEmergencyEventService> _mockEmergencyEventService;
    private readonly EmergencyEventController _controller;

    public EmergencyEventControllerTests()
    {
        _mockEmergencyEventService = new Mock<IEmergencyEventService>();
        _controller = new EmergencyEventController(_mockEmergencyEventService.Object);
    }

    [Fact]
    public async Task GetEmergencyEventAsync_ReturnsOk_WhenEventExists()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var emergencyEvent = new EmergencyDetailsDto
        {
            Id = eventId,
            Description = "Sample Event",
            Location = "Sample Location",
            Latitude = (decimal)35.6,
            Longitude = (decimal)139.6,
            Severity = Severity.Critical,
            Status = Status.New,
            Type = EmergencyType.Avalanche,
            ReportedBy = Guid.NewGuid(),
            ReportedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ParticipantsUsernames = new[] { "user1", "user2" },
            ParticipantsCount = 2
        };

        _mockEmergencyEventService.Setup(s => s.GetEmergencyEventByIdAsync(eventId))
                                  .ReturnsAsync(ApiResponse<EmergencyDetailsDto>.Success(emergencyEvent));

        // Act
        var result = await _controller.GetEmergencyEventAsync(eventId);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<EmergencyDetailsResponse>(actionResult.Value);
        Assert.True(response.IsSuccess);
    }

    [Fact]
    public async Task GetEmergencyEventAsync_ReturnsNotFound_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _mockEmergencyEventService.Setup(s => s.GetEmergencyEventByIdAsync(eventId))
                                  .ReturnsAsync(ApiResponse<EmergencyDetailsDto>.Failure("Not found"));

        // Act
        var result = await _controller.GetEmergencyEventAsync(eventId);

        // Assert
        var actionResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<EmergencyEventResponse>(actionResult.Value);
        Assert.False(response.IsSuccess);
    }
}

public class ApiResponse<T>
{
    public static OperationResult<EmergencyEventDto> Failure(string notFound)
    {
        throw new NotImplementedException();
    }

    public static OperationResult<EmergencyEventDto> Success(EmergencyDetailsDto emergencyEvent)
    {
        throw new NotImplementedException();
    }
}
