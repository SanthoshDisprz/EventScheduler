using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.DataAccess;
using DisprzTraining.Data;
using Appointments;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public class AppointmentsBL : IAppointmentsBL
    {
        private readonly IAppointmentsDAL _appointmentsDAL;
        public AppointmentsBL(IAppointmentsDAL appointmentsDAL)
        {
            _appointmentsDAL = appointmentsDAL;
        }
        //In GET call, convert ISO time to client's local time
        private List<Appointment> convertToLocalTime(List<Appointment> appointmentList)
        {
            List<Appointment> appointmentInLocalTimeStamp = new List<Appointment>();
            foreach(var appointment in appointmentList)
            {
               var updatedAppointment = new Appointment();
               updatedAppointment.Id = appointment.Id;
               updatedAppointment.Title = appointment.Title;
               updatedAppointment.StartTime = appointment?.StartTime.Value.AddMinutes(-appointment.TimeZoneOffset);
               updatedAppointment.EndTime = appointment?.EndTime.Value.AddMinutes(-appointment.TimeZoneOffset);
               updatedAppointment.Description = appointment.Description;
               updatedAppointment.CreatedBy = appointment.CreatedBy;
               updatedAppointment.GuestsList = appointment.GuestsList;
               updatedAppointment.Location = appointment.Location;
               updatedAppointment.TimeZoneOffset = appointment.TimeZoneOffset;
               
               appointmentInLocalTimeStamp.Add(updatedAppointment);
                
            }
            return appointmentInLocalTimeStamp;
        }

        public List<Appointment> GetAppointments(DateTime? date, int?timeZoneOffset, string ?duration)
        {
            var allAppointments =  _appointmentsDAL.GetAllAppointments();
            DateTime now = DateTime.UtcNow;
            if ((date != null && duration == null)||(date!=null && duration!=null))
            {
            var appointmentList =  _appointmentsDAL.GetAppointmentsByDate(date.Value, timeZoneOffset.Value);
           var appointmentsInLocalTime = convertToLocalTime(appointmentList);
            return appointmentsInLocalTime;
            }
            else if (duration?.ToLower() == "month" && date==null)
            {
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddSeconds(-1);
                var appointmentsForCurrentMonth = allAppointments.Where(appointment => appointment.StartTime >= startDate && appointment.StartTime <= endDate).ToList();
                var appointmentsInLocalTime = convertToLocalTime(appointmentsForCurrentMonth);
                return appointmentsInLocalTime;
            }
            else if (duration?.ToLower() == "year" && date==null)
            {
                var startDate = new DateTime(now.Year, 1, 1);
                var endDate = startDate.AddYears(1).AddSeconds(-1);
                var appointmentsForCurrentYear = allAppointments.Where(appointment => appointment.StartTime >= startDate && appointment.StartTime <= endDate).ToList();
                var appointmentsInLocalTime = convertToLocalTime(appointmentsForCurrentYear);
                return appointmentsInLocalTime;
            }
            else if (duration?.ToLower() == "week" && date==null)
            {
                var startDate = now.AddDays(-(int)now.DayOfWeek-1);
                var endDate = startDate.AddDays(7).AddSeconds(-1);
                var appointmentsForCurrentWeek = allAppointments.Where(appointment => appointment.StartTime >= startDate && appointment.StartTime <= endDate).ToList();
                var appointmentsInLocalTime = convertToLocalTime(appointmentsForCurrentWeek);
                return appointmentsInLocalTime;
            }
            // else if(date!=null && duration!=null) throw new Exception("Enter Either appointment date or duration. Both should not be entered");
            else throw new Exception("Either Appointment Date or Duration is mandatory");

        }

        public bool AppointmentConflictCheck(AddAppointment appointment)
        {

            bool isConflict = false;
            foreach (var meet in AppointmentsStore.AppointmentList)
            {
                if ((meet.StartTime < appointment.EndTime) && (appointment.StartTime < meet.EndTime))
                {
                    isConflict = true;
                }
            }

            return isConflict;
        }
        public bool CreateAppointment(AddAppointment appointment)
        {
            var currentTime = DateTime.UtcNow;
            if (appointment.StartTime == appointment.EndTime) throw new Exception("Appointment Start time and End time should not be same");
            else if (appointment.StartTime > appointment.EndTime) throw new Exception("Appointment Start time should be greater than End time");
            else if(appointment.StartTime < currentTime) throw new Exception("Cannot create appointment for past time");
            return _appointmentsDAL.CreateAppointment(appointment);
        }

        public bool DeleteAppointment(Guid id)
        {
            bool isValidAppointmentId = false;
            var appointment = AppointmentsStore.AppointmentList.Find(appointment => appointment.Id == id);
            var currentDateAndTime = DateTime.UtcNow;
            var appointmentStartDateAndTime = appointment?.StartTime;
            if (currentDateAndTime > appointmentStartDateAndTime)
            {
                throw new Exception("Cannot Delete Older Appointments");
            }
            if (appointment != null)
            {
                _appointmentsDAL.DeleteAppointment(appointment);
                isValidAppointmentId = true;
            }
            return isValidAppointmentId;
        }
        public bool UpdateAppointmentConflictCheck(Guid id, AddAppointment appointment)
        {

            foreach (var meet in AppointmentsStore.AppointmentList)
            {
                if (id == meet.Id) { continue; }
                if ((meet.StartTime < appointment.EndTime) && (appointment.StartTime < meet.EndTime))
                {
                    return true;
                }

            }
            return false;

        }
        public bool UpdateAppointment(Guid id, AddAppointment appointment)
        {
            if (appointment.StartTime == appointment.EndTime) throw new Exception("Appointment Start time and End time should not be same");
            else if (appointment.StartTime > appointment.EndTime) throw new Exception("Appointment Start time should be greater than End time");
            var currentDateAndTime = DateTime.UtcNow;
            var appointmentStartDateAndTime = appointment.StartTime;
            if (currentDateAndTime > appointmentStartDateAndTime)
            {
                throw new Exception("Cannot Update Older Appointments");
            }

            var appointmentToBeUpdated = AppointmentsStore.AppointmentList.Find(appointment => appointment.Id == id);
            if (appointmentToBeUpdated == null)
            {
                return false;
            }
            _appointmentsDAL.UpdateAppointment(appointmentToBeUpdated, appointment);
            return true;
        }
    }
}








