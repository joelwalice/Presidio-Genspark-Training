using JobPortalAPI.DTOs;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using JobPortalAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileService fileService, ILogger<FileController> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "JobSeeker,Admin")]
        public async Task<IActionResult> Upload([FromForm] ResumeUploadDto dto)
        {
            _logger.LogInformation($"Uploading file for user : {User.Identity.Name}");
            var fileName = await _fileService.UploadAsync(dto);
            if (string.IsNullOrEmpty(fileName))
            {
                _logger.LogError("File upload failed.");
                return BadRequest("File upload failed.");
            }
            _logger.LogInformation($"File {fileName} uploaded successfully by user: {User.Identity.Name}");
            return Ok(new { fileName });
        }

        [HttpGet("{fileName}")]
        [Authorize(Roles = "JobSeeker,Admin")]
        public async Task<IActionResult> Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _logger.LogError("File name is null or empty.");
                return BadRequest("File name cannot be null or empty.");
            }
            _logger.LogInformation($"Downloading file: {fileName} for user: {User.Identity.Name}");
            var file = await _fileService.DownloadAsync(fileName);
            if (file == null) return NotFound();
            _logger.LogInformation($"File {fileName} downloaded successfully by user: {User.Identity.Name}");
            return file;
        }
    }
}
