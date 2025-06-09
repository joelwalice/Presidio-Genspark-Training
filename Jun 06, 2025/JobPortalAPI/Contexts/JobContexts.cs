using System;
using System.ComponentModel.DataAnnotations;
using JobPortalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JobPortalAPI.Contexts
{
    public class JobContexts : DbContext
    {
        public JobContexts(DbContextOptions<JobContexts> options) : base(options)
        {
        }

        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ResumeDocument> ResumeDocuments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=JobPortalDB;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobSeeker>()
                .HasOne(js => js.User)
                .WithOne(u => u.JobSeeker)
                .HasForeignKey<JobSeeker>(js => js.UserId)
                .HasConstraintName("FK_JobSeeker_User")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id)
                .HasName("PK_UserId");

            modelBuilder.Entity<Recruiter>()
                .HasOne(rc => rc.Company)
                .WithMany(c => c.Recruiters)
                .HasForeignKey(rc => rc.CompanyId)
                .HasConstraintName("FK_Recruiter_Company")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ResumeDocument>()
                .HasOne(rd => rd.JobSeeker)
                .WithMany(js => js.ResumeDocuments)
                .HasForeignKey(rd => rd.JobSeekerId)
                .HasConstraintName("FK_ResumeDocument_JobSeeker")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Company)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CompanyId)
                .HasConstraintName("FK_Job_Company")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobEmploymentType>()
                .HasKey(jet => jet.Id);

            modelBuilder.Entity<JobEmploymentType>()
                .HasOne(jet => jet.Job)
                .WithMany(j => j.JobEmploymentTypes)
                .HasForeignKey(jet => jet.JobId)
                .HasConstraintName("FK_JobEmploymentType_Job")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobEmploymentType>()
                .HasOne(jet => jet.EmploymentType)
                .WithMany(et => et.JobEmploymentTypes)
                .HasForeignKey(jet => jet.EmploymentTypeId)
                .HasConstraintName("FK_JobEmploymentType_EmploymentType")
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}