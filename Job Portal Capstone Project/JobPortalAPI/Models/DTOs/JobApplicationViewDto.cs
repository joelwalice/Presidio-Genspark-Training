namespace JobPortalAPI.Models
{
    public class JobApplicationViewDto
    {
        public Guid JobSeekerId { get; set; }
        public string JobSeekerName { get; set; } = string.Empty;
        public Guid ResumeDocumentId { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}