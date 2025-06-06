using FirstAPI.Models;
using FirstAPI.Interfaces;
using System.Threading.Tasks;
using FirstAPI.Models.DTOs.PatientAddRequestDto;
using FirstAPI.Repositories;


namespace FirstAPI.Services
{
    public class PatientService
    {
        public IRepository<int, Patient> _patientRepository;
        public IRepository<int, Appointment> _appointmentRepository;
        public PatientService(IRepository<int, Patient> PatientRepository,
                              IRepository<int, Appointment> appointmentRepository)
        {
            _patientRepository = PatientRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Patient> AddPatient(PatientAddRequestDto patientDto)
        {
            var newPatient = new Patient
            {
                Name = patientDto.Name,
                Age = patientDto.Age,
                Email = patientDto.Email,
                Phone = patientDto.Phone,
            };
            var createdPatient = await _patientRepository.Add(newPatient);
            if (patientDto.Appointments != null) {
                foreach (var appointment in patientDto.Appointments)
                {
                    var newAppointment = new Appointment
                    {
                        AppointmentDateTime = appointment.AppointmentDateTime,
                        DoctorId = appointment.DoctorId,
                        PatientId = createdPatient.Id
                    };
                    await _appointmentRepository.Add(newAppointment);
                }
            }
            return createdPatient;
        }

    }
}