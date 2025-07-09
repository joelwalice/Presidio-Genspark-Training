namespace JobPortalAPI.Models.DTOs;

public class JobApplicationDto
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Guid JobSeekerId { get; set; }
    public Guid ResumeDocumentId { get; set; }
    public DateTime AppliedAt { get; set; }
    public int JobStatus { get; set; }
}
