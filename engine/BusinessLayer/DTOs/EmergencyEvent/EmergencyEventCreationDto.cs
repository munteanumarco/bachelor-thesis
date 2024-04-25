using DataAccessLayer.Entities;

namespace BusinessLayer.DTOs.EmergencyEvent;

public class EmergencyEventCreationDto
{
    public string? Description { get; set; }
    public string Location { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public Severity Severity { get; set; }
    public EmergencyType Type { get; set; }
}