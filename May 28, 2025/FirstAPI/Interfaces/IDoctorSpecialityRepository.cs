using FirstAPI.Models;
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using System.Collections.Generic;


namespace FirstAPI.Interfaces
{
    public interface IDoctorSpecialityRepository : IRepository<int, DoctorSpeciality>
    {
        Task<IEnumerable<DoctorSpeciality>> GetByDoctorId(int doctorId);
        Task<IEnumerable<DoctorSpeciality>> GetBySpecialtyId(int specialtyId);
        Task<IEnumerable<DoctorSpeciality>> GetAllActiveDoctorSpecialties();
        Task<DoctorSpeciality> GetByDoctorAndSpecialty(int doctorId, int specialtyId);
    }
}