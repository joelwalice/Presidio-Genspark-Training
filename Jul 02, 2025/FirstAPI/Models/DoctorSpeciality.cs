using System.ComponentModel.DataAnnotations;

namespace FirstAPI.Models
{
    public class DoctorSpeciality
    {
        [Key]
        public int SerialNumber { get; set; }
        public int DoctorId { get; set; }
        public int SpecialityId { get; set; }
        public Speciality? Speciality { get; set; }
        public Doctor? Doctor { get; set; }
    }
}