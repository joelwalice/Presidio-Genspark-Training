using FirstAPI.Models;
using FirstAPI.Interfaces;
using System.Threading.Tasks;
using FirstAPI.Models.DTOs.PatientAddRequestDto;
using FirstAPI.Repositories;
using FirstAPI.Services;
using AutoMapper;

namespace FirstAPI.Services
{
    public class PatientService
    {
        public IRepository<int, Patient> _patientRepository;
        public IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        public PatientService(IRepository<int, Patient> PatientRepository,
                              IRepository<int, Appointment> appointmentRepository,
                              IRepository<string, User> userRepository,
                              IMapper mapper,
                              IEncryptionService encryptionService)
        {
            _patientRepository = PatientRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }

        public async Task<Patient> AddPatient(PatientAddRequestDto patientDto)
        {
            try
            {
                var user = _mapper.Map<PatientAddRequestDto, User>(patientDto);
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = patientDto.Password 
                });
                user.Password = encryptedData.EncryptedData;
                user.HashKey = encryptedData.HashKey;
                user.Role = "Patient";
                user = await _userRepository.Add(user);

                var newPatient = new Patient
                {
                    Name = patientDto.Name,
                    Age = patientDto.Age,
                    Email = patientDto.Email,
                    Phone = patientDto.Phone,
                    User = user 
                };
                var createdPatient = await _patientRepository.Add(newPatient);

                if (patientDto.Appointments != null)
                {
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}