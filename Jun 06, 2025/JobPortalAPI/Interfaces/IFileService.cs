using JobPortalAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalAPI.Services
{
    public interface IFileService
    {
        Task<string> UploadAsync(ResumeUploadDto dto);
        Task<FileContentResult?> DownloadAsync(string fileName);
    }
}
