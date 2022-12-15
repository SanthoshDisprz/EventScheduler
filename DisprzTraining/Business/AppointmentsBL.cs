using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.DataAccess;
using DisprzTraining.Data;
using Appointments;

namespace DisprzTraining.Business
{
    public class AppointmentsBL: IAppointmentsBL
    {
        private readonly IAppointmentDAL _appointmentdal;
        public AppointmentsBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentdal = appointmentDAL;
        }
        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _appointmentdal.GetAllAppointments();
        }
        public async Task<List<Appointment>> GetAppointments(string eventDate)
        {
            DateTime dateTime;
            if(DateTime.TryParse(eventDate, out dateTime))
            {
            return await _appointmentdal.GetAppointments(eventDate);
            }
            else{
                throw new Exception("Invalid Input Type");
            }
            // return await _appointmentdal.GetAppointments(eventDate);
        }
        public async Task<bool> CreateAppointment(Appointment appointment)
        {
            if(appointment.AppointmentStartTime==appointment.AppointmentEndTime) throw new Exception("Start time and end time should not be same");
            else if(appointment.AppointmentStartTime>appointment.AppointmentEndTime) throw new Exception("Appointment Start time should be greater than End time");

            bool isConflict=false;
            foreach(var x in AppointmentStore.AppointmentList.ToList())
            {
                isConflict = (x.AppointmentStartTime<=appointment.AppointmentEndTime) && (appointment.AppointmentStartTime<=x.AppointmentEndTime); 
            }
            if(isConflict == false)
            {
             return await _appointmentdal.CreateAppointment(appointment);
            }
            else
                return false;
               
             

        }
        public async Task<bool> DeleteAppointment(int id)
        {
            bool isValidAppointmentId=false;
            var appointment = AppointmentStore.AppointmentList.Find(u=>u.Id==id);
            if(appointment!=null)
            {
            await _appointmentdal.DeleteAppointment(appointment);
            isValidAppointmentId=true;
            }
            return isValidAppointmentId;
        }
        public async Task<bool> UpdateAppointment(int id, Appointment appointment)
        {
            if(id!=appointment.Id || appointment==null)
            {
                return false;
            }
            var appointmentToBeUpdated = AppointmentStore.AppointmentList.Find(u=>u.Id==id);
            await _appointmentdal.UpdateAppointment(appointmentToBeUpdated, appointment);
            return true;
        }
    }
}