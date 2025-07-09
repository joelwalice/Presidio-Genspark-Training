namespace JobPortalAPI.Models
{
    public enum JobStatus
    {
        Applied,
        Accepted,
        Hired,
        Rejected
    }
    public class JobApplication
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }
        public Guid JobSeekerId { get; set; }

        public Guid ResumeDocumentId { get; set; }

        public DateTime AppliedAt { get; set; }
        public JobStatus JobStatus { get; set; }
        public Job? Job { get; set; }
        public ResumeDocument? ResumeDocument { get; set; }
        public JobSeeker? User { get; set; }
    }
}
