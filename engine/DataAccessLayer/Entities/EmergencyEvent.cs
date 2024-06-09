using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities;

public class EmergencyEvent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string Location { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public EmergencyType Type { get; set; }
    public Severity Severity { get; set; }
    public Status Status { get; set; }
    public Guid? ReportedBy { get; set; }
    public DateTime ReportedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public LandCoverAnalysis? LandCoverAnalysis {get; set;}
}