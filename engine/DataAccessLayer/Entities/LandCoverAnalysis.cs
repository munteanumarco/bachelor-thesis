namespace DataAccessLayer.Entities;

public class LandCoverAnalysis
{
    public Guid Id { get; set; }
    public Guid EmergencyEventId { get; set; }
    public LandCoverAnalysisStatus Status { get; set; }
    public string? RawImage { get; set; }
    public string? ProcessedImage { get; set; }
    public DateTime? TriggeredAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public EmergencyEvent EmergencyEvent { get; set; }
}