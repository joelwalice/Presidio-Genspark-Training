using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface IAppointmentRepository : IRepository<int, Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync();
        Task<Appointment> GetAppointmentDetailsAsync(int appointmentId);
    }
}