using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoUploadAPI.Contexts;
using VideoUploadAPI.Models;
using VideoUploadAPI.Models.DTOs;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

namespace VideoUploadAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoUploadController : ControllerBase
    {
        private readonly VideoContexts _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public VideoUploadController(VideoContexts context, BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _containerName = configuration.GetValue<string>("AzureBlobStorage:ContainerName");
        }

        [HttpGet("")]
        public async Task<IActionResult> GetVideos()
        {
            try
            {
                var videos = await _context.VideoUploads
                        .OrderByDescending(v => v.UploadDate)
                        .Select(v => new
                        {
                            v.Id,
                            v.Title,
                            v.Description,
                            v.UploadDate,
                            v.BlobUrl
                        }).ToListAsync();

                return Ok(videos);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes (in a real app, use a logging framework)
                Console.WriteLine($"Error fetching videos: {ex.Message}");
                // Return a 500 Internal Server Error status with a generic message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadVideo([FromForm] VideoUploadDto videoUploadDto)
        {
            if (videoUploadDto == null || videoUploadDto.VideoFile == null)
            {
                return BadRequest("Invalid upload data.");
            }

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var blobName = $"{Guid.NewGuid()}-{videoUploadDto.VideoFile.FileName}";
                var blobClient = containerClient.GetBlobClient(blobName);

                using (var stream = videoUploadDto.VideoFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = videoUploadDto.VideoFile.ContentType });
                }

                var blobUrl = blobClient.Uri.ToString();

                // var sasBuilder = new BlobSasBuilder
                // {
                //     BlobContainerName = _containerName,
                //     BlobName = blobName,
                //     Resource = "b", // "b" for blob
                //     StartsOn = DateTimeOffset.UtcNow,
                //     ExpiresOn = DateTimeOffset.UtcNow.AddHours(1), // Token valid for 1 hour
                // };
                // sasBuilder.SetPermissions(BlobSasPermissions.Read);
                // var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;
                // blobUrl = $"{blobClient.Uri.GetLeftPart(UriPartial.Path)}{sasToken}";

                var trainingVideo = new VideoUpload
                {
                    Title = videoUploadDto.Title,
                    Description = videoUploadDto.Description,
                    UploadDate = DateTime.UtcNow,
                    BlobUrl = blobUrl
                };

                _context.VideoUploads.Add(trainingVideo);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Video uploaded successfully!", Video = trainingVideo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}