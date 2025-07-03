namespace FirstAPI.Models.DTOs
{
    public class AppointmentAddRequestDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}