using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using DisprzTraining.Models;
using Appointments;

namespace DisprzTraining.Business
{
    public interface IAppointmentsBL
    {
        Task<List<Appointment>> GetAllAppointments();
        Task<List<Appointment>> GetAppointments(string eventDate);
        Task<bool> CreateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(int id);
        Task<bool> UpdateAppointment(int id, Appointment appointment);
    }
}