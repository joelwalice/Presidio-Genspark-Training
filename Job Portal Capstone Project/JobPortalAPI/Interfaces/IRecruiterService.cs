using System;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Interfaces
{
    public interface IRecruiterService
    {
        Task<Recruiter> GetRecruiterByIdAsync(Guid id);
        Task<IEnumerable<Recruiter>> GetAllRecruitersAsync();
        Task<Recruiter> AddRecruiterAsync(RecruiterAddRequestDto recruiter);
        Task<Recruiter> UpdateRecruiterAsync(RecruiterUpdateRequestDto recruiter);
        Task<bool> DeleteRecruiterAsync(Guid id);
    }
}