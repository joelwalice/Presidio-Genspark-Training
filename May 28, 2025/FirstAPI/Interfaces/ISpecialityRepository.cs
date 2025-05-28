using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface ISpecialityRepository : IRepository<int, Speciality>
    {
        Task<IEnumerable<Speciality>> GetAllActiveSpecialties();
        Task<Speciality> GetByName(string name);
        Task<IEnumerable<Speciality>> GetByDoctorId(int doctorId);
        Task<IEnumerable<Speciality>> GetByPatientId(int patientId);
    }
}