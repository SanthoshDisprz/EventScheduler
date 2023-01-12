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
        List<Appointment> GetAppointments(DateTime? date, int? timeZoneOffset, string ?duration);
        bool AppointmentConflictCheck(AddAppointment appointment);
        bool CreateAppointment(AddAppointment appointment);
        bool DeleteAppointment(Guid id);
        bool UpdateAppointmentConflictCheck(Guid id, AddAppointment appointment);
        bool UpdateAppointment(Guid id, AddAppointment appointment);
        // List<Appointment> convertToLocalTime(List<Appointment> appointmentList);
    }
}