using DataAccessLayer.Entities;

namespace BusinessLayer.DTOs.EmergencyEvent;

public class EmergencyEventMarkerDto
{
    public Guid Id { get; set; }
    public Severity Severity { get; set; }
    public EmergencyType Type { get; set; }
    public Status Status { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}