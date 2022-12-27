using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using DisprzTraining.Models;
using Appointments;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentsBL
    {
        Task<List<Appointment>> GetAllAppointments();
        Task<List<Appointment>> GetAppointmentsByDate(string appointmentDate);
        Task<List<Appointment>> GetAppointmentById(Guid id);
        Task<bool> AppointmentConflictCheck(Appointment appointment);
        Task<bool> CreateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(Guid id);
        Task<bool> UpdateAppointmentConflictCheck(Guid id, UpdateAppointment appointment);
        Task<bool> UpdateAppointment(Guid id, UpdateAppointment appointment);
    }
}