using JobPortalAPI.Contexts;
using JobPortalAPI.DTOs;
using JobPortalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalAPI.Services
{
    public class FileService : IFileService
    {
        private readonly JobContexts _context;

        public FileService(JobContexts context)
        {
            _context = context;
        }

        public async Task<string> UploadAsync(ResumeUploadDto dto)
        {
            using var memoryStream = new MemoryStream();
            await dto.File.CopyToAsync(memoryStream);
            var content = memoryStream.ToArray();

            var resume = new ResumeDocument
            {
                Id = Guid.NewGuid(),
                FileName = dto.File.FileName,
                FileType = dto.File.ContentType,
                Content = content,
                FileSize = dto.File.Length,
                JobSeekerId = dto.JobSeekerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ResumeDocuments.Add(resume);

            var jobSeeker = await _context.JobSeekers
                .FirstOrDefaultAsync(js => js.Id == dto.JobSeekerId);

            if (jobSeeker == null)
                throw new Exception("Job Seeker not found");

            jobSeeker.DefaultResumeId = resume.Id;
            jobSeeker.UpdatedAt = DateTime.UtcNow;

            _context.JobSeekers.Update(jobSeeker);
            await _context.SaveChangesAsync();

            return resume.FileName;
        }

        public async Task<FileContentResult?> DownloadAsync(string fileName)
        {
            var file = await _context.ResumeDocuments
                .FirstOrDefaultAsync(f => f.FileName == fileName);

            if (file == null) return null;

            return new FileContentResult(file.Content, file.FileType)
            {
                FileDownloadName = file.FileName
            };
        }

        public async Task<string> DeleteFileAsync(Guid resumeId)
        {
            var file = await _context.ResumeDocuments
                .FirstOrDefaultAsync(f => f.Id == resumeId);

            if (file == null) return "No such file";

            var jobSeekerId = file.JobSeekerId;

            _context.ResumeDocuments.Remove(file);
            await _context.SaveChangesAsync();

            var jobSeeker = await _context.JobSeekers
                .Include(js => js.ResumeDocuments)
                .FirstOrDefaultAsync(js => js.Id == jobSeekerId);

            if (jobSeeker != null && jobSeeker.DefaultResumeId == resumeId)
            {
                var nextResume = jobSeeker.ResumeDocuments
                    .OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefault();

                jobSeeker.DefaultResumeId = nextResume?.Id;
                jobSeeker.UpdatedAt = DateTime.UtcNow;
                _context.JobSeekers.Update(jobSeeker);
                await _context.SaveChangesAsync();
            }

            return "File Removed Successfully";
        }
        public async Task<string> SetDefaultResumeAsync(Guid jobSeekerId, Guid resumeId)
        {
            var jobSeeker = await _context.JobSeekers
                .FirstOrDefaultAsync(js => js.Id == jobSeekerId);

            var resumeExists = await _context.ResumeDocuments
                .AnyAsync(r => r.Id == resumeId && r.JobSeekerId == jobSeekerId);

            if (jobSeeker == null || !resumeExists)
                return "Invalid JobSeeker or Resume";

            jobSeeker.DefaultResumeId = resumeId;
            jobSeeker.UpdatedAt = DateTime.UtcNow;
            _context.JobSeekers.Update(jobSeeker);
            await _context.SaveChangesAsync();

            return "Default Resume Updated";
        }
        public async Task<FileContentResult?> GetResumeByIdAsync(Guid resumeId)
        {
            var resume = await _context.ResumeDocuments
                .FirstOrDefaultAsync(r => r.Id == resumeId);

            if (resume == null) return null;

            return new FileContentResult(resume.Content, resume.FileType)
            {
                FileDownloadName = resume.FileName
            };
        }

    }
}
