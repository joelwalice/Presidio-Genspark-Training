using BlobAPI.Services;
using Microsoft.AspNetCore.Mvc;
using BlobAPI.Models.DTOs;

namespace BlobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly BlobService _blobService;

        public BlobController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto dto)
        {
            var file = dto.File;

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            await _blobService.UploadFile(stream, file.FileName);

            return Ok($"File '{file.FileName}' uploaded successfully.");
        }



        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var stream = await _blobService.DownloadFile(fileName);

            if (stream == null)
                return NotFound("File not found in blob storage.");

            return File(stream, "application/octet-stream", fileName);
        }
    }
}
