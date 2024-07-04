using DataAccessLayer.Entities;

namespace BusinessLayer.DTOs.Analysis;

public class LandCoverAnalysisDto
{
    public Guid EmergencyEventId { get; set; }
    public LandCoverAnalysisStatus Status { get; set; }
}