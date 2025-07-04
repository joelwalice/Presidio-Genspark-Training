using JobPortalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortalAPI.Services
{
    public interface IJobServices
    {
        Task<IEnumerable<JobGetRequestDto>> GetAllJobsAsync();
        Task<JobGetRequestDto?> GetJobByIdAsync(Guid id);
        Task<JobAddRequestDto> CreateJobAsync(JobAddRequestDto jobDto);
        Task<JobAddRequestDto?> UpdateJobAsync(Guid id, JobAddRequestDto jobDto);

        Task<string> ApplyForJobWithResumeIdAsync(Guid jobId, Guid jobSeekerId, Guid resumeDocumentId);

        Task<bool> DeleteJobAsync(Guid id);
    }
}
