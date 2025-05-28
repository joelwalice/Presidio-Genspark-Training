using FirstAPI.Models;
using System;
using System.Collections.Generic;   
using System.Threading.Tasks;

namespace FirstAPI.Interfaces
{
    public interface IDoctorRepository : IRepository<int, Doctor>
    {
        Task<Doctor> GetDoctorByName(string name);
        Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(int specialityId);
        Task<IEnumerable<Doctor>> GetDoctorsByExperience(float yearsOfExperience);
        Task<IEnumerable<Doctor>> GetAllActiveDoctors();
    }
}