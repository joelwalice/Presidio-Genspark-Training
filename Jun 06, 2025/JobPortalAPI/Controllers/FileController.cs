using JobPortalAPI.DTOs;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JobPortalAPI.Models.DTOs;

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
        [HttpDelete("{resumeId}")]
        [Authorize(Roles = "Admin, JobSeeker")]
        public async Task<IActionResult> Delete(Guid resumeId)
        {
            _logger.LogInformation($"Deleting the File with Id : {resumeId}");
            var output = await _fileService.DeleteFileAsync(resumeId);
            if (output == "No such file") return NotFound(new { message = "No Such File Present!!" });
            return Ok(new { message = output });
        }

        [HttpPut("set-default")]
        [Authorize(Roles = "Admin, JobSeeker")]
        public async Task<IActionResult> SetDefaultResume([FromBody] SetDefaultResumeDto dto)
        {
            var result = await _fileService.SetDefaultResumeAsync(dto.JobSeekerId, dto.ResumeId);
            if (result == "Invalid JobSeeker or Resume")
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("resume/{resumeId}")]
        [Authorize(Roles = "Admin, JobSeeker, Recruiter")]
        public async Task<IActionResult> GetResumeById(Guid resumeId)
        {
            _logger.LogInformation($"Fetching the Resume with Id : {resumeId}");
            var resume = await _fileService.GetResumeByIdAsync(resumeId);
            Console.WriteLine("----> Resume "  + resume?.FileContents);
            if (resume == null)
            {
                _logger.LogError($"No resume found with Id: {resumeId}");
                return NotFound(new { message = "No resume found with the provided ID." });
            }
            _logger.LogInformation($"Resume with Id : {resumeId} fetched successfully");
            return Ok(resume);
        }
    }
}
