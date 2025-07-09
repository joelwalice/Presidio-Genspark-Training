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
    public class ResumeDocumentRepository : IRepository<Guid, ResumeDocument>
    {
        private readonly JobContexts _context;

        public ResumeDocumentRepository(JobContexts context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ResumeDocument> GetByIdAsync(Guid id)
        {
            try
            {
                var resumeDocument = await _context.ResumeDocuments
                    .Include(rd => rd.JobSeeker)
                    .FirstOrDefaultAsync(rd => rd.Id == id);

                if (resumeDocument == null)
                {
                    throw new KeyNotFoundException($"ResumeDocument with ID {id} not found.");
                }

                return resumeDocument;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving ResumeDocument with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ResumeDocument>> GetAllAsync()
        {
            try
            {
                return await _context.ResumeDocuments
                    .Include(rd => rd.JobSeeker)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all ResumeDocuments: {ex.Message}", ex);
            }
        }

        public async Task<ResumeDocument> AddAsync(ResumeDocument entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _context.ResumeDocuments.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error adding ResumeDocument: {ex.Message}", ex);
            }
        }

        public async Task<ResumeDocument> UpdateAsync(ResumeDocument entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                var existingDocument = await _context.ResumeDocuments.FindAsync(entity.Id);
                if (existingDocument == null)
                {
                    throw new KeyNotFoundException($"ResumeDocument with ID {entity.Id} not found.");
                }

                entity.UpdatedAt = DateTime.UtcNow;
                entity.CreatedAt = existingDocument.CreatedAt; // Preserve original creation date
                
                _context.Entry(existingDocument).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Error updating ResumeDocument: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var resumeDocument = await _context.ResumeDocuments.FindAsync(id);
                if (resumeDocument == null)
                {
                    return false;
                }

                _context.ResumeDocuments.Remove(resumeDocument);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting ResumeDocument with ID {id}: {ex.Message}", ex);
            }
        }
    }
}