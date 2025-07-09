using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;

public interface IJobApplicationService
{
    Task<List<AppliedJobDto>> GetAppliedJobsByJobSeekerIdAsync(Guid jobSeekerId);
    Task<List<JobApplicationDto>> GetApplicationsByJobIdAsync(Guid jobId);
    Task<JobApplicationDto> UpdateApplicationStatusAsync(Guid jobSeekerId, UpdateJobStatusDto jobStatus);
}