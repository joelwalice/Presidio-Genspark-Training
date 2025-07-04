namespace JobPortalAPI.Models.DTOs
{
    public class SetDefaultResumeDto
    {
        public Guid JobSeekerId { get; set; }
        public Guid ResumeId { get; set; }
    }

}