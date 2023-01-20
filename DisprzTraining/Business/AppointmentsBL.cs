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
        //In GET call, convert ISO time to user's local time
        public List<Appointment> ConvertToLocalTime(List<Appointment> appointmentList, int timeZoneOffset)
        {
            List<Appointment> appointmentInLocalTimeStamp = new List<Appointment>();
            foreach (var appointment in appointmentList)
            {
                var updatedAppointment = new Appointment();
                updatedAppointment.Id = appointment.Id;
                updatedAppointment.Title = appointment.Title;
                updatedAppointment.StartTime = appointment.StartTime.Value.AddMinutes(-timeZoneOffset);
                updatedAppointment.EndTime = appointment.EndTime.Value.AddMinutes(-timeZoneOffset);
                updatedAppointment.Description = appointment.Description;
                updatedAppointment.CreatedBy = appointment.CreatedBy;
                updatedAppointment.GuestsList = appointment.GuestsList;
                updatedAppointment.Location = appointment.Location;
                appointmentInLocalTimeStamp.Add(updatedAppointment);

            }
            return appointmentInLocalTimeStamp;
        }

        public List<Appointment> GetAppointments(DateTime? startTime, DateTime? endTime, int timeZoneOffset)
        {
            if (startTime > endTime) throw new Exception("Start time should be lesser than End time");

            else if (startTime == null || endTime == null) throw new Exception("Both Start time and End time are mandatory for getting appointments");
            else if (startTime.Value == endTime.Value) throw new Exception("Start time and End time should not be equal");

            var appointmentsList = _appointmentsDAL.GetAppointments(startTime, endTime);
            var appointmentsInLocalTime = ConvertToLocalTime(appointmentsList, timeZoneOffset);
            return appointmentsInLocalTime;
        }

        public bool AppointmentConflictCheck(AddAppointment appointment)
        {
            var allAppointments = _appointmentsDAL.GetAllAppointments();

            bool isConflict = false;
            if (allAppointments.Any())
            {
                foreach (var meet in allAppointments)
                {
                    if ((meet.StartTime < appointment.EndTime) && (appointment.StartTime < meet.EndTime))
                    {
                        isConflict = true;
                    }
                }
            }

            return isConflict;
        }
        public bool CreateAppointment(AddAppointment appointment)
        {
            var currentTime = DateTime.UtcNow;
            if (appointment.StartTime > appointment.EndTime) throw new Exception("Appointment Start time should be greater than End time");
            else if (appointment.StartTime < currentTime) throw new Exception("Cannot create appointment for past time");
            else if (appointment.StartTime == null || appointment.EndTime == null) throw new Exception("Both Start time and End time are mandatory for creating appointments");
            else if (appointment.StartTime.Value == appointment.EndTime.Value) throw new Exception("Appointment Start time and End time should not be same");
            var isConflict = AppointmentConflictCheck(appointment);
            if (isConflict)
            {
                return false;
            }
            return _appointmentsDAL.CreateAppointment(appointment);
        }

        public bool DeleteAppointment(Guid id)
        {
            var allAppointments = _appointmentsDAL.GetAllAppointments();
            bool isValidAppointmentId = false;
            var appointment = allAppointments.Find(appointment => appointment.Id == id);
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
            var allAppointments = _appointmentsDAL.GetAllAppointments();
            if (allAppointments.Any())
            {
                foreach (var meet in allAppointments)
                {
                    if (id == meet.Id) { continue; }
                    if ((meet.StartTime < appointment.EndTime) && (appointment.StartTime < meet.EndTime))
                    {
                        return true;
                    }

                }
            }
            return false;

        }
        public bool UpdateAppointment(Guid id, AddAppointment appointment)
        {
            var currentDateAndTime = DateTime.UtcNow;
            var appointmentToBeUpdated = _appointmentsDAL.GetAppointmentById(id);
            if (appointmentToBeUpdated == null)
            {
                return false;
            }

            else if (appointmentToBeUpdated.StartTime.Value < currentDateAndTime) throw new Exception("Cannot Update Past Appointment");

            if (appointment.StartTime < currentDateAndTime)
            {
                throw new Exception("Cannot Update Appointment to Past time");
            }
            else if (appointment.StartTime > appointment.EndTime) throw new Exception("Appointment Start time should be greater than End time");
            else if (appointment.StartTime == null || appointment.EndTime == null) throw new Exception("Both Start time and End time are mandatory for updating appointments");
            else if (appointment.StartTime.Value == appointment.EndTime.Value) throw new Exception("Appointment Start time and End time should not be same");
            return _appointmentsDAL.UpdateAppointment(appointmentToBeUpdated, appointment);

        }


    }
}








