using System;
using System.ComponentModel.DataAnnotations;
using VideoUploadAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VideoUploadAPI.Contexts
{
    public class VideoContexts : DbContext
    {
        public VideoContexts(DbContextOptions<VideoContexts> options)
            : base(options)
        {
        }

        public DbSet<VideoUpload> VideoUploads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoUpload>()
                .Property(v => v.UploadDate);
            base.OnModelCreating(modelBuilder);
        }
    }
}