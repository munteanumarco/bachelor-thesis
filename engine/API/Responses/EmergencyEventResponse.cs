using BusinessLayer.DTOs.EmergencyEvent;

namespace API.Responses;

public class EmergencyEventResponse : BaseResponse
{
    public EmergencyEventDto EmergencyEvent { get; set; }

    private EmergencyEventResponse(bool isSuccess, IEnumerable<string> errorMessages, EmergencyEventDto emergencyEventDto)
        : base(isSuccess, errorMessages)
    {
        EmergencyEvent = emergencyEventDto;
    }

    public static EmergencyEventResponse Success(EmergencyEventDto emergencyEventDto)
    {
        return new EmergencyEventResponse(true, new List<string>(), emergencyEventDto);
    }

    public new static EmergencyEventResponse Failure(IEnumerable<string> errorMessages)
    {
        return new EmergencyEventResponse(false, errorMessages, null);
    }
}