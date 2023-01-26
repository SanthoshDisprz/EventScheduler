using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.Data;
// using Appointments;
using DisprzTraining.Models;

namespace DisprzTraining.DataAccess
{
    public class AppointmentsDAL : IAppointmentsDAL
    {
        public List<Appointment> GetAllAppointments()
        {
            return AppointmentsStore.AppointmentList;
        }

        public List<Appointment> GetAppointments(DateTime startTime, DateTime endTime)
        {
            return AppointmentsStore.AppointmentList.Where(appointment => appointment.StartTime.Value >= startTime && appointment.StartTime.Value <= endTime).ToList();
        }
        public Appointment GetAppointmentById(Guid id)
        {
            return AppointmentsStore.AppointmentList.Find(appointment => appointment.Id == id);
        }

        public bool CreateAppointment(AddAppointment appointment)
        {
            var appointmentWithId = new Appointment()
            {
                Id = Guid.NewGuid(),
                Title = appointment.Title,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Description = appointment.Description,
                CreatedBy = appointment.CreatedBy,
                GuestsList = appointment.GuestsList,
                Location = appointment.Location
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
            appointmentToBeUpdated.Title = appointment.Title;
            appointmentToBeUpdated.StartTime = appointment.StartTime;
            appointmentToBeUpdated.EndTime = appointment.EndTime;
            appointmentToBeUpdated.Description = appointment.Description;
            appointmentToBeUpdated.CreatedBy = appointment.CreatedBy;
            appointmentToBeUpdated.GuestsList = appointment.GuestsList;
            appointmentToBeUpdated.Location = appointment.Location;
            return true;
        }
        public List<Appointment> GetAppointmentsByTitle(string title)
        {
                return AppointmentsStore.AppointmentList.Where(appointment => appointment.Title.ToLower().Contains(title.ToLower())).OrderBy(appointment => appointment.StartTime).ToList();
        }
    }
}