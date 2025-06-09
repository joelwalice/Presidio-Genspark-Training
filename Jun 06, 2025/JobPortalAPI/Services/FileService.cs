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
            await _context.SaveChangesAsync();

            var jobSeeker = await _context.JobSeekers
                .Where(js => js.Id == dto.JobSeekerId)
                .FirstOrDefaultAsync();
            if (jobSeeker != null)
            {
                jobSeeker.ResumeId = resume.Id;
                jobSeeker.UpdatedAt = DateTime.UtcNow;
                _context.JobSeekers.Update(jobSeeker);
                await _context.SaveChangesAsync();
            }

            else
            {
                throw new Exception("Job Seeker not found");
            }

            _context.JobSeekers.Update(jobSeeker);
            await _context.SaveChangesAsync();

            return resume.FileName;
        }

        public async Task<FileContentResult?> DownloadAsync(string fileName)
        {
            var file = await _context.ResumeDocuments
                .Where(f => f.FileName == fileName)
                .FirstOrDefaultAsync();

            if (file == null) return null;

            return new FileContentResult(file.Content, file.FileType)
            {
                FileDownloadName = file.FileName
            };
        }
    }
}
