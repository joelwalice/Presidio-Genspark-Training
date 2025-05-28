using FirstAPI.Models;
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models.DTOs;
using System.Collections.Generic;

namespace FirstAPI.Models.DTOs.PatientAddRequestDto
{
    public class PatientAddRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<AppointmentAddRequestDto>? Appointments { get; set; } = new List<AppointmentAddRequestDto>();
    }
}