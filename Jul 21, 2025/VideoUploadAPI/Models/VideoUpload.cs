using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoUploadAPI.Models
{
    public class VideoUpload
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(500)]
        public string BlobUrl { get; set; }
    }
}