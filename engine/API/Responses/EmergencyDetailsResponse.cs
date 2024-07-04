using BusinessLayer.DTOs.EmergencyEvent;

namespace API.Responses;

public class EmergencyDetailsResponse  : BaseResponse
{
    public EmergencyDetailsDto Details { get; set; }

    private EmergencyDetailsResponse(bool isSuccess, IEnumerable<string> errorMessages, EmergencyDetailsDto emergencyDetailsDto)
        : base(isSuccess, errorMessages)
    {
        Details = emergencyDetailsDto;
    }

    public static EmergencyDetailsResponse Success(EmergencyDetailsDto emergencyDetailsDto)
    {
        return new EmergencyDetailsResponse(true, new List<string>(), emergencyDetailsDto);
    }

    public new static EmergencyDetailsResponse Failure(IEnumerable<string> errorMessages)
    {
        return new EmergencyDetailsResponse(false, errorMessages, null);
    }
}