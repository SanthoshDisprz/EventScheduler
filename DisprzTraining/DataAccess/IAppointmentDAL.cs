using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointments;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        Task<List<Appointment>> GetAllAppointments();
        Task<List<Appointment>> GetAppointments(string eventDate);
        Task<bool> CreateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(Appointment appointmenToBeUpdated, Appointment appointment);
        
    }
}