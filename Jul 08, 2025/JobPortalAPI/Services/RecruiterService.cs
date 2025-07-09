using System;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Services
{
    public class RecruiterService : IRecruiterService
    {
        private readonly JobContexts _context;

        public RecruiterService(JobContexts context)
        {
            _context = context;
        }

        public async Task<Recruiter> GetRecruiterByIdAsync(Guid id)
        {
            return await _context.Recruiters.FindAsync(id);
        }

        public async Task<IEnumerable<Recruiter>> GetAllRecruitersAsync()
        {
            return await _context.Recruiters.ToListAsync();
        }

        public async Task<Recruiter> AddRecruiterAsync(RecruiterAddRequestDto recruiterDto)
        {
            try
            {
            if (recruiterDto == null)
            {
                throw new ArgumentNullException(nameof(recruiterDto));
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = recruiterDto.Name,
                Email = recruiterDto.Email,
                IsDeleted = false,
                Role = "Recruiter",
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(recruiterDto.Password, 13),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Company? company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Name == recruiterDto.CompanyName);

            if (company == null)
            {
                company = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = recruiterDto.CompanyName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    
                };
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
            }

            var recruiter = new Recruiter
            {
                Id = Guid.NewGuid(),
                Name = recruiterDto.Name,
                Email = recruiterDto.Email,
                PhoneNumber = recruiterDto.PhoneNumber,
                DateOfBirth = recruiterDto.DateOfBirth,
                Address = recruiterDto.Address,
                CompanyId = company.Id,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Recruiters.Add(recruiter);
            await _context.SaveChangesAsync();

            return recruiter;
            }
            catch (DbUpdateException ex)
            {
            throw new InvalidOperationException($"Error adding Recruiter: {ex.Message}", ex);
            }
        }

        public async Task<Recruiter> UpdateRecruiterAsync(RecruiterUpdateRequestDto recruiterDto)
        {
            var recruiter = await _context.Recruiters.FindAsync(recruiterDto.Id);
            if (recruiter == null) return null;

            recruiter.Name = recruiterDto.Name;
            recruiter.Email = recruiterDto.Email;
            recruiter.PhoneNumber = recruiterDto.PhoneNumber;
            recruiter.DateOfBirth = recruiterDto.DateOfBirth;
            recruiter.Address = recruiterDto.Address;

            _context.Recruiters.Update(recruiter);
            await _context.SaveChangesAsync();
            return recruiter;
        }

        public async Task<bool> DeleteRecruiterAsync(Guid id)
        {
            var recruiter = await _context.Recruiters.FindAsync(id);
            if (recruiter == null) return false;

            _context.Recruiters.Remove(recruiter);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}