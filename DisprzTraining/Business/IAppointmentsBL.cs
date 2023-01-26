using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using DisprzTraining.Models;
// using Appointments;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentsBL
    {
        List<Appointment> GetAppointments(DateTime? startTime, DateTime? endTime, int timeZoneOffset);
        PaginatedResponse GetAppointmentsByTitle(string? title, int pageNumber, int pageSize, int timeZoneOffset);
        bool AppointmentConflictCheck(AddAppointment appointment);
        bool CreateAppointment(AddAppointment appointment);
        bool DeleteAppointment(Guid id);
        bool UpdateAppointmentConflictCheck(Guid id, AddAppointment appointment);
        bool UpdateAppointment(Guid id, AddAppointment appointment);
        List<Appointment> ConvertToLocalTime(List<Appointment> appointmentList, int timeZoneOffset);
    }
}