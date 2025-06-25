using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models.DTOs;
using JobPortalAPI.Repositories;

namespace JobPortalAPI.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly JobSeekerRepository _JobSeekerRepository;
        private readonly JobContexts _context;
        public JobSeekerService(JobContexts context)
        {
            _context = context;
        }
        public async Task<JobSeekerAddRequestDto> GetJobSeekerByIdAsync(Guid id)
        {
            try
            {
                var jobSeeker = await _context.JobSeekers
                    .Include(js => js.User)
                    .FirstOrDefaultAsync(js => js.Id == id);

                if (jobSeeker == null)
                {
                    throw new KeyNotFoundException($"JobSeeker with ID {id} not found.");
                }

                return new JobSeekerAddRequestDto
                {
                    Name = jobSeeker.Name,
                    Email = jobSeeker.Email,
                    PhoneNumber = jobSeeker.PhoneNumber,
                    Address = jobSeeker.Address,
                    Password = jobSeeker.User.PasswordHash,
                    DateOfBirth = jobSeeker.DateOfBirth
                };
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving JobSeeker with ID {id}: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<JobSeekerAddRequestDto>> GetAllJobSeekersAsync()
        {
            try
            {
                var jobSeekers = await _context.JobSeekers
                    .Include(js => js.User)
                    .ToListAsync();

                return jobSeekers.Select(js => new JobSeekerAddRequestDto
                {
                    Id = js.Id,
                    Name = js.Name,
                    Email = js.Email,
                    PhoneNumber = js.PhoneNumber,
                    DateOfBirth = js.DateOfBirth,
                    Address = js.Address,
                    Password = js.User.PasswordHash,
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all JobSeekers: {ex.Message}", ex);
            }
        }
        public async Task<JobSeeker> AddJobSeekerAsync(JobSeekerAddRequestDto jobSeekerDto)
        {
            try
            {
                if (jobSeekerDto == null)
                {
                    throw new ArgumentNullException(nameof(jobSeekerDto));
                }
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = jobSeekerDto.Email,
                    Name = jobSeekerDto.Name,
                    Role = "JobSeeker",
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(jobSeekerDto.Password, 13),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var jobSeeker = new JobSeeker
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Name = jobSeekerDto.Name,
                    Email = jobSeekerDto.Email,
                    PhoneNumber = jobSeekerDto.PhoneNumber,
                    DateOfBirth = jobSeekerDto.DateOfBirth,
                    PasswordHash = user.PasswordHash,
                    Address = jobSeekerDto.Address,
                };

                
                _context.JobSeekers.Add(jobSeeker);
                await _context.SaveChangesAsync();

                return jobSeeker;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error adding JobSeeker: {ex.Message}", ex);
            }
        }
        public async Task<JobSeeker> UpdateJobSeekerAsync(JobSeekerUpdateRequestDto jobSeekerDto)
        {
            try
            {
                if (jobSeekerDto == null)
                {
                    throw new ArgumentNullException(nameof(jobSeekerDto));
                }

                var jobSeeker = await _context.JobSeekers.FindAsync(jobSeekerDto.Id);
                if (jobSeeker == null)
                {
                    throw new KeyNotFoundException($"JobSeeker with ID {jobSeekerDto.Id} not found.");
                }

                jobSeeker.Name = jobSeekerDto.Name;
                jobSeeker.PhoneNumber = jobSeekerDto.PhoneNumber;
                jobSeeker.Address = jobSeekerDto.Address;

                _context.JobSeekers.Update(jobSeeker);
                await _context.SaveChangesAsync();

                return jobSeeker;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error updating JobSeeker: {ex.Message}", ex);
            }
        }
        public async Task<bool> DeleteJobSeekerAsync(Guid id)
        {
            try
            {
                var jobSeeker = await _context.JobSeekers.FindAsync(id);
                if (jobSeeker == null)
                {
                    throw new KeyNotFoundException($"JobSeeker with ID {id} not found.");
                }

                _context.JobSeekers.Remove(jobSeeker);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error deleting JobSeeker: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<ResumeDocument>> GetResumeDocumentsByJobSeekerIdAsync(Guid jobSeekerId)
        {
            try
            {
                var resumes = await _context.ResumeDocuments
                    .Where(rd => rd.JobSeekerId == jobSeekerId)
                    .ToListAsync();

                if (resumes == null || !resumes.Any())
                {
                    throw new KeyNotFoundException($"No resumes found for JobSeeker with ID {jobSeekerId}.");
                }

                return resumes;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving resumes for JobSeeker with ID {jobSeekerId}: {ex.Message}", ex);
            }
        }
    }
}

