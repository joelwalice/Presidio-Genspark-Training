using JobPortalAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalAPI.Services
{
    public interface IFileService
    {
        Task<string> UploadAsync(ResumeUploadDto dto);
        Task<FileContentResult?> DownloadAsync(string fileName);
        Task<string> DeleteFileAsync(Guid resumeId);
        Task<string> SetDefaultResumeAsync(Guid jobSeekerId, Guid resumeId);
        Task<FileContentResult?> GetResumeByIdAsync(Guid resumeId);
    }
}
