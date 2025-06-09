using JobPortalAPI.Contexts;
using JobPortalAPI.DTOs;
using JobPortalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalAPI.Services
{
    public class JobServices : IJobServices
    {
        private readonly JobContexts _context;

        public JobServices(JobContexts context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobAddRequestDto>> GetAllJobsAsync()
        {
            return await _context.Jobs
                .Select(j => new JobAddRequestDto
                {
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    ExpiryDate = j.ExpiryDate,
                    CompanyName = j.CompanyName,
                    Salary = j.Salary,
                    Requirements = j.Requirements,
                    CompanyId = j.CompanyId,
                    RecruiterId = j.RecruiterId
                }).ToListAsync();
        }

        public async Task<JobAddRequestDto?> GetJobByIdAsync(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null) return null;

            return new JobAddRequestDto
            {
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                ExpiryDate = job.ExpiryDate,
                CompanyName = job.CompanyName,
                Salary = job.Salary,
                Requirements = job.Requirements,
                CompanyId = job.CompanyId,
                RecruiterId = job.RecruiterId
            };
        }

        public async Task<JobAddRequestDto> CreateJobAsync(JobAddRequestDto dto)
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                PostedDate = DateTime.UtcNow,
                ExpiryDate = dto.ExpiryDate,
                CompanyName = dto.CompanyName,
                Salary = dto.Salary,
                Requirements = dto.Requirements,
                CompanyId = dto.CompanyId,
                RecruiterId = dto.RecruiterId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<JobAddRequestDto?> UpdateJobAsync(Guid id, JobAddRequestDto dto)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null) return null;

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.Location = dto.Location;
            job.ExpiryDate = dto.ExpiryDate;
            job.CompanyName = dto.CompanyName;
            job.Salary = dto.Salary;
            job.Requirements = dto.Requirements;
            job.CompanyId = dto.CompanyId;
            job.RecruiterId = dto.RecruiterId;
            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<bool> DeleteJobAsync(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null) return false;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
