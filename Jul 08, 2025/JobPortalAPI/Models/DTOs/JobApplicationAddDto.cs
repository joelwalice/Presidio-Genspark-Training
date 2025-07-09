namespace JobPortalAPI.Models
{
    public class JobApplicationAddDto
    {
        public Guid JobSeekerId { get; set; }
        public Guid JobId { get; set; }
        public Guid ResumeDocumentId { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.Now;

    }
}