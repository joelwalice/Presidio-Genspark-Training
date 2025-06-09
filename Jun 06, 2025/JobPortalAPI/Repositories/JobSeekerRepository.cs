using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models.DTOs;

namespace JobPortalAPI.Repositories
{
    public class JobSeekerRepository : IRepository<Guid, JobSeeker>
    {
        private readonly JobContexts _context;

        public JobSeekerRepository(JobContexts context)
        {
            _context = context;
        }

        public async Task<JobSeeker> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.JobSeekers.FirstOrDefaultAsync(js => js.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving JobSeeker with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<JobSeeker>> GetAllAsync()
        {
            try
            {
                return await _context.JobSeekers.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all JobSeekers: {ex.Message}", ex);
            }
        }

        public async Task<JobSeeker> AddAsync(JobSeeker entity)
        {
            try
            {
                _context.JobSeekers.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding JobSeeker: {ex.Message}", ex);
            }
        }

        public async Task<JobSeeker> UpdateAsync(JobSeeker entity)
        {
            try
            {
                _context.JobSeekers.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating JobSeeker: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var jobSeeker = await _context.JobSeekers.FindAsync(id);
                if (jobSeeker == null)
                {
                    return false;
                }
                _context.JobSeekers.Remove(jobSeeker);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting JobSeeker with ID {id}: {ex.Message}", ex);
            }
        }
    }
}