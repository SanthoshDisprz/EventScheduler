using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Appointments;
using DisprzTraining.Models;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentsDAL
    {
        List<Appointment> GetAllAppointments();
        List<Appointment> GetAppointments(DateTime startTime, DateTime endTime);
        List<Appointment> GetAppointmentsByTitle(string title);
        Appointment GetAppointmentById(Guid id);
        bool CreateAppointment(AddAppointment appointment);
        bool DeleteAppointment(Appointment appointment);
        bool UpdateAppointment(Appointment appointmenToBeUpdated, AddAppointment appointment);

    }
}