using BusinessLayer.DTOs.EmergencyEvent;

namespace API.Responses;

public class EmergencyEventMarkerResponse : BaseResponse
{
    public IEnumerable<EmergencyEventMarkerDto> Markers { get; set; }

    private EmergencyEventMarkerResponse(bool isSuccess, IEnumerable<string> errorMessages, IEnumerable<EmergencyEventMarkerDto> emergencyEventMarkers)
        : base(isSuccess, errorMessages)
    {
        Markers = emergencyEventMarkers;
    }

    public static EmergencyEventMarkerResponse Success(IEnumerable<EmergencyEventMarkerDto> emergencyEventMarkers)
    {
        return new EmergencyEventMarkerResponse(true, new List<string>(), emergencyEventMarkers);
    }

    public new static EmergencyEventMarkerResponse Failure(IEnumerable<string> errorMessages)
    {
        return new EmergencyEventMarkerResponse(false, errorMessages, null);
    }
}