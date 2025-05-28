using FirstAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using System.Threading.Tasks;
using System;
using FirstAPI.Repositories;

namespace FirstAPI.Services
{
    public class DoctorService : IDoctorService
    {
        public IRepository<int, Doctor> _doctorRepository;
        public IRepository<int, Speciality> _specialityRepository;
        public IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;

        public DoctorService(IRepository<int, Doctor> doctorRepository,
                            IRepository<int, Speciality> specialityRepository,
                            IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
        }
        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
        {
            var newDoctor = new Doctor
            {
                Name = doctor.Name,
                YearsOfExperience = doctor.YearsOfExperience,
                Status = "Active",
            };

            var createdDoctor = await _doctorRepository.Add(newDoctor);

            if (doctor.Specialities != null)
            {
                foreach (var speciality in doctor.Specialities)
                {
                    var newSpeciality = new Speciality
                    {
                        Name = speciality.Name,
                        Status = "Active"
                    };
                    var createdSpeciality = await _specialityRepository.Add(newSpeciality);

                    var doctorSpeciality = new DoctorSpeciality
                    {
                        DoctorId = createdDoctor.Id,
                        SpecialityId = createdSpeciality.Id,
                    };
                    await _doctorSpecialityRepository.Add(doctorSpeciality);
                }
            }

            return createdDoctor;

        }

        public async Task<Doctor> GetDoctByName(string name)
        {
            var allDoctors = await _doctorRepository.GetAll();
            var doctor = allDoctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (doctor == null)
            {
                throw new Exception($"Doctor with name {name} not found.");
            }
            return doctor;
        }

        public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
        {
            var allSpecialities = await _specialityRepository.GetAll();
            var specialityEntity = allSpecialities.FirstOrDefault(s => s.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase));
            if (specialityEntity == null)
            {
                throw new Exception($"Speciality {speciality} not found.");
            }
            var allDoctorSpecialities = await _doctorSpecialityRepository.GetAll();
            var doctorIds = allDoctorSpecialities
                .Where(ds => ds.SpecialityId == specialityEntity.Id)
                .Select(ds => ds.DoctorId)
                .ToList();
            var allDoctors = await _doctorRepository.GetAll();
            var doctors = allDoctors
                .Where(d => doctorIds.Contains(d.Id))
                .ToList();
            if (doctors.Count == 0)
            {
                throw new Exception($"No doctors found for speciality {speciality}.");
            }
            return doctors;
        }

    }
}