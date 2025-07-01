using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Contexts;
using FirstAPI.Interfaces;


namespace FirstAPI.Misc
{
    public class OtherFunctionalitiesImplementation : IOtherContextFunctionities
    {
        private readonly ClinicContext _clinicContext;

        public OtherFunctionalitiesImplementation(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity)
        {
            var result = await _clinicContext.GetDoctorsBySpeciality(specilaity);
            return result;
        }
    }
}