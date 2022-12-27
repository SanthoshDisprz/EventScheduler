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
    public class AppointmentsBL: IAppointmentsBL
    {
        private readonly IAppointmentDAL _appointmentdal;
        public AppointmentsBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentdal = appointmentDAL;
        }
        public async Task<List<Appointment>> GetAllAppointments()
        {
            var appointmentList = await _appointmentdal.GetAllAppointments();
            var sortedAppointmentList = appointmentList.OrderBy(appointment=>appointment.AppointmentStartDateTime).ToList();
            return sortedAppointmentList;
        }
        public async Task<List<Appointment>> GetAppointmentsByDate(string appointmentDate)
        {
            DateTime dateTime;
            if(DateTime.TryParse(appointmentDate, out dateTime))
            {
            var appointmentList = await _appointmentdal.GetAppointmentsByDate(appointmentDate);
            var sortedAppointmentList = appointmentList.OrderBy(appointment=>appointment.AppointmentStartDateTime).ToList();
            return sortedAppointmentList;
            }
            else{
                throw new Exception("Invalid Input Type for Appointment Date. Appointment Date should be of Date format");
            }
        }
        public async Task<List<Appointment>> GetAppointmentById(Guid id)
        {
            return await _appointmentdal.GetAppointmentById(id);
        }
        public Task<bool> AppointmentConflictCheck(Appointment appointment)
        {
            
            bool isConflict=false;
            foreach(var meet in AppointmentStore.AppointmentList)
            {
                if((meet.AppointmentStartDateTime<=appointment.AppointmentEndDateTime) && (appointment.AppointmentStartDateTime<=meet.AppointmentEndDateTime)){
                    isConflict = true;
                }
            }
           
                return Task.FromResult(isConflict);
        }
        public async Task<bool> CreateAppointment(Appointment appointment)
        {
            if(appointment.AppointmentStartDateTime==appointment.AppointmentEndDateTime) throw new Exception("Appointment Start time and End time should not be same");
            else if(appointment.AppointmentStartDateTime>appointment.AppointmentEndDateTime) throw new Exception("Appointment Start time should be greater than End time");
            DateTime dateTime;
            if(DateTime.TryParse(appointment.AppointmentDate, out dateTime))
            {
            return await _appointmentdal.CreateAppointment(appointment);
            }
            else{
                throw new Exception("Invalid Input Type for Appointment Date. Appointment Date should be of Date format");
            }
           
        }
        
        public async Task<bool> DeleteAppointment(Guid id)
        {
            bool isValidAppointmentId=false;
            var appointment = AppointmentStore.AppointmentList.Find(appointment=>appointment.Id==id);
             var currentDateAndTime = DateTime.UtcNow;
            var appointmentStartDateAndTime = appointment?.AppointmentStartDateTime;
            if(currentDateAndTime>appointmentStartDateAndTime){
                throw new Exception("Cannot Delete Older Appointments");
            }
            if(appointment!=null)
            {
            await _appointmentdal.DeleteAppointment(appointment);
            isValidAppointmentId=true;
            }
            return isValidAppointmentId;
        }
        public Task<bool> UpdateAppointmentConflictCheck(Guid id, UpdateAppointment appointment)
        {
           
            // bool isConflict=false;
            foreach(var meet in AppointmentStore.AppointmentList)
            {
                if(id==meet.Id){ continue;}
                if((meet.AppointmentStartDateTime<=appointment.AppointmentEndDateTime) && (appointment.AppointmentStartDateTime<=meet.AppointmentEndDateTime)){
                    // isConflict = true;
                    return Task.FromResult(true);
                }
            
            }
           return Task.FromResult(false);
                
        }
        public async Task<bool> UpdateAppointment(Guid id, UpdateAppointment appointment)
        {
            if(appointment.AppointmentStartDateTime==appointment.AppointmentEndDateTime) throw new Exception("Appointment Start time and End time should not be same");
            else if(appointment.AppointmentStartDateTime>appointment.AppointmentEndDateTime) throw new Exception("Appointment Start time should be greater than End time");
            var currentDateAndTime = DateTime.UtcNow;
            var appointmentStartDateAndTime = appointment.AppointmentStartDateTime;
            if(currentDateAndTime>appointmentStartDateAndTime){
                throw new Exception("Cannot Update Older Appointments");
            }

            var appointmentToBeUpdated = AppointmentStore.AppointmentList.Find(appointment=>appointment.Id==id);
            if(appointmentToBeUpdated==null){
                return false;
            }
            await _appointmentdal.UpdateAppointment(appointmentToBeUpdated, appointment);
            return true;
        }
    }
}


























// if(appointment.AppointmentStartTime==appointment.AppointmentEndTime) throw new Exception("Start time and end time should not be same");
//             else if(appointment.AppointmentStartTime>appointment.AppointmentEndTime) throw new Exception("Appointment Start time should be greater than End time");

//             bool isConflict=false;
//             foreach(var x in AppointmentStore.AppointmentList.ToList())
//             {
//                 isConflict = (x.AppointmentStartTime<=appointment.AppointmentEndTime) && (appointment.AppointmentStartTime<=x.AppointmentEndTime); 
//             }
//             if(isConflict == false)
//             {
//              return await _appointmentdal.CreateAppointment(appointment);
//             }
//             else
//                 return false;








//  bool isConflict=false;
//             foreach(var x in AppointmentStore.AppointmentList)
//             {
//                 if((x.AppointmentStartTime<=appointment.AppointmentEndTime) && (appointment.AppointmentStartTime<=x.AppointmentEndTime)){ 
//                 return isConflict=true;
//                 }
//                 else{
//                     await _appointmentdal.CreateAppointment(appointment);
//                     return isConflict = false;
//                 }
//             }
//             if(isConflict == false)
//             {
              
//               return true;
//             }
//                 return isConflict;