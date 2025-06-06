using System;
using System.Threading.Tasks;
using FirstAPI.Contexts;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Services
{
    public class AppointmentService
    {
        private readonly ClinicContext _context;

        public AppointmentService(ClinicContext context)
        {
            _context = context;
        }

        public async Task CancelAppointment(string appointmentNumber, int doctorId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentNumber)
                            ?? throw new Exception("Appointment not found");

            var doctor = await _context.Doctors.FindAsync(doctorId)
                        ?? throw new Exception("Doctor not found");

            appointment.Status = "Cancelled";
            await _context.SaveChangesAsync();
        }
    }
}