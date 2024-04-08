using BusinessLayer.DTOs.EmergencyEvent;

namespace API.Responses;

public class EmergencyEventResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> ErrorMessages { get; set; }
    public EmergencyEventDto EmergencyEvent { get; set; }

    public EmergencyEventResponse(bool isSuccess, IEnumerable<string> errorMessages, EmergencyEventDto emergencyEventDto)
    {
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        EmergencyEvent = emergencyEventDto;
    }

    public static EmergencyEventResponse Success(EmergencyEventDto emergencyEventDto)
    {
        return new EmergencyEventResponse(true, new List<string>(), emergencyEventDto);
    }

    public static EmergencyEventResponse Failure(IEnumerable<string> errorMessages)
    {
        return new EmergencyEventResponse(false, errorMessages, null);
    }
}