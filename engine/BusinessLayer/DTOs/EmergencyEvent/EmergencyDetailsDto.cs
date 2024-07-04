using DataAccessLayer.Entities;

namespace BusinessLayer.DTOs.EmergencyEvent;

public class EmergencyDetailsDto : EmergencyEventDto
{
    public string? ReportedByUsername { get; set; }
    public int ParticipantsCount { get; set; }
    public IEnumerable<string> ParticipantsUsernames { get; set; }
}