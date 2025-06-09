using System;

using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models.DTOs
{
    public class RecruiterUpdateRequestDto : RecruiterAddRequestDto
    {
        public Guid Id { get; set; }
    }
}