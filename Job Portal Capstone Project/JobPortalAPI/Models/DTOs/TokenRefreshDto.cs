using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models.DTOs
{
    public class TokenRefreshDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        public string AccessToken { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public Guid UserId { get; set; }

        public string Role { get; set; } = string.Empty;
    }
}