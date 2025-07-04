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
    public class CompanyRepository : IRepository<Guid, Company>
    {
        private readonly JobContexts _context;

        public CompanyRepository(JobContexts context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Company> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Companies
                    .Include(c => c.Recruiters)
                    .Include(c => c.Jobs)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving Company with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                return await _context.Companies
                    .Include(c => c.Recruiters)
                    .Include(c => c.Jobs)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all Companies: {ex.Message}", ex);
            }
        }

        public async Task<Company> AddAsync(Company entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
                
                _context.Companies.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error adding Company: {ex.Message}", ex);
            }
        }

        public async Task<Company> UpdateAsync(Company entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                var existingCompany = await _context.Companies.FindAsync(entity.Id);
                if (existingCompany == null)
                {
                    throw new KeyNotFoundException($"Company with ID {entity.Id} not found.");
                }

                entity.UpdatedAt = DateTime.UtcNow;
                entity.CreatedAt = existingCompany.CreatedAt;

                _context.Entry(existingCompany).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error updating Company: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var company = await _context.Companies.FindAsync(id);
                if (company == null)
                {
                    return false;
                }

                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting Company with ID {id}: {ex.Message}", ex);
            }
        }
    }
}