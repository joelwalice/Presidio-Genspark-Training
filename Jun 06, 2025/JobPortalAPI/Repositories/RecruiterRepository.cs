using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.Interfaces;

namespace JobPortalAPI.Repositories
{
    public class RecruiterRepository : IRepository<Guid, Recruiter>
    {
        private readonly JobContexts _context;

        public RecruiterRepository(JobContexts context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Recruiter> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Recruiters
                    .Include(r => r.Company)
                    .FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving Recruiter with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Recruiter>> GetAllAsync()
        {
            try
            {
                return await _context.Recruiters
                    .Include(r => r.Company)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all Recruiters: {ex.Message}", ex);
            }
        }

        public async Task<Recruiter> AddAsync(Recruiter entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _context.Recruiters.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error adding Recruiter: {ex.Message}", ex);
            }
        }

        public async Task<Recruiter> UpdateAsync(Recruiter entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                var existingRecruiter = await _context.Recruiters.FindAsync(entity.Id);
                if (existingRecruiter == null)
                {
                    throw new KeyNotFoundException($"Recruiter with ID {entity.Id} not found.");
                }

                entity.UpdatedAt = DateTime.UtcNow;
                entity.CreatedAt = existingRecruiter.CreatedAt;

                _context.Entry(existingRecruiter).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error updating Recruiter: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var recruiter = await _context.Recruiters.FindAsync(id);
                if (recruiter == null)
                {
                    return false;
                }

                _context.Recruiters.Remove(recruiter);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting Recruiter with ID {id}: {ex.Message}", ex);
            }
        }
    }
}