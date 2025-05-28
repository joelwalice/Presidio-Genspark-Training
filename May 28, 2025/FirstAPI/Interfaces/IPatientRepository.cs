using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface IPatientRepository : IRepository<int, Patient>
    {
        Task<IEnumerable<Patient>> GetPatientsByAgeAsync(int minAge);
        Task<IEnumerable<Patient>> GetPatientsByEmailAsync(string email);
        Task<Patient> GetPatientWithAppointmentsAsync(int patientId);
    }   
}