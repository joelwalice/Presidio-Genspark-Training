﻿using AppointmentApp.Interfaces;
using AppointmentApp.Models;
using AppointmentApp.Repositories;
using AppointmentApp.Services;

namespace AppointmentApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            IRepository<int, Appointment> appointmentRepository = new AppointmentRepository();
            IAppointmentService appointmentService = new AppointmentService(appointmentRepository);
            ManageAppointment manageAppointment = new ManageAppointment(appointmentService);
            manageAppointment.Start();
        }
    }
}