using JobPortalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortalAPI.Services
{
    public interface IJobServices
    {
        Task<IEnumerable<JobAddRequestDto>> GetAllJobsAsync();
        Task<JobAddRequestDto?> GetJobByIdAsync(Guid id);
        Task<JobAddRequestDto> CreateJobAsync(JobAddRequestDto jobDto);
        Task<JobAddRequestDto?> UpdateJobAsync(Guid id, JobAddRequestDto jobDto);
        Task<bool> DeleteJobAsync(Guid id);
    }
}
