using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.Data;
using Appointments;
using DisprzTraining.Models;

namespace DisprzTraining.DataAccess
{
    public class AppointmentsDAL : IAppointmentsDAL
    {
        public List<Appointment> GetAllAppointments()
        {
            return AppointmentsStore.AppointmentList;
        }
        public List<Appointment> GetAppointmentsByDate(DateTime appointmentDate, int timeZoneOffset)
        {
            // var localDateTime = appointmentDate.AddMinutes(-(timeZoneOffset));
            // var startTime = new DateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day, 0, 0, 0);
            // var endTime = new DateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day, 23, 59, 59, 999);
            var startTime = appointmentDate.AddMinutes(timeZoneOffset);
            var endTime = startTime.AddHours(24).AddSeconds(-1);
            return AppointmentsStore.AppointmentList.Where(appointment => appointment.StartTime>=startTime && appointment.EndTime<=endTime).ToList();
        }

        public bool CreateAppointment(AddAppointment appointment)
        {
            var appointmentWithId = new Appointment(){
                Id=Guid.NewGuid(),
                Title=appointment.Title,
                StartTime=appointment.StartTime,
                EndTime=appointment.EndTime,
                TimeZoneOffset=appointment.TimeZoneOffset,
                Description=appointment.Description,
                CreatedBy=appointment.CreatedBy,
                GuestsList=appointment.GuestsList,
                Location=appointment.Location
            };
            AppointmentsStore.AppointmentList.Add(appointmentWithId);
            return true;
            
        }
        public bool DeleteAppointment(Appointment appointment)
        {
            AppointmentsStore.AppointmentList.Remove(appointment);
            return true;
        }
        public bool UpdateAppointment(Appointment appointmentToBeUpdated, AddAppointment appointment)
        {
            appointmentToBeUpdated.Title=appointment.Title;
            appointmentToBeUpdated.StartTime=appointment.StartTime;
            appointmentToBeUpdated.EndTime=appointment.EndTime;
            appointmentToBeUpdated.TimeZoneOffset=appointment.TimeZoneOffset;
            appointmentToBeUpdated.Description=appointment.Description;
            appointmentToBeUpdated.CreatedBy=appointment.CreatedBy;
            appointmentToBeUpdated.GuestsList=appointment.GuestsList;
            appointmentToBeUpdated.Location=appointment.Location;
            return true;
        }
    }
}