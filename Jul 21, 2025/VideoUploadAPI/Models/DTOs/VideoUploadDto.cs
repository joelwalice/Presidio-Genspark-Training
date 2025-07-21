using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoUploadAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace VideoUploadAPI.Models.DTOs
{
    public class VideoUploadDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public IFormFile VideoFile { get; set; }
    }
}