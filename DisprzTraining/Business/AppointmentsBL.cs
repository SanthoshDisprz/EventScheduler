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
        public async Task<List<Appointment>> GetAppointments(string appointmentDate)
        {
            DateTime dateTime;
            if(DateTime.TryParse(appointmentDate, out dateTime))
            {
            return await _appointmentdal.GetAppointments(appointmentDate);
            }
            else{
                throw new Exception("Invalid Input Type");
            }
        }
        public async Task<List<Appointment>> GetAppointmentById(Guid id)
        {
            return await _appointmentdal.GetAppointmentById(id);
        }
        public Task<bool> AppointmentConflictCheck(Appointment appointment)
        {
            if(appointment.AppointmentStartTime==appointment.AppointmentEndTime) throw new Exception("Appointment Start time and End time should not be same");
            else if(appointment.AppointmentStartTime>appointment.AppointmentEndTime) throw new Exception("Appointment Start time should be greater than End time");
            bool isConflict=false;
            foreach(var meet in AppointmentStore.AppointmentList.ToList())
            {
                if((meet.AppointmentStartTime<=appointment.AppointmentEndTime) && (appointment.AppointmentStartTime<=meet.AppointmentEndTime)){
                    isConflict = true;
                }
            }
           
                return Task.FromResult(isConflict);
        }
        public async Task<bool> CreateAppointment(Appointment appointment)
        {
           return await _appointmentdal.CreateAppointment(appointment);
        }
        
        public async Task<bool> DeleteAppointment(Guid id)
        {
            bool isValidAppointmentId=false;
            var appointment = AppointmentStore.AppointmentList.Find(appointment=>appointment.Id==id);
            if(appointment!=null)
            {
            await _appointmentdal.DeleteAppointment(appointment);
            isValidAppointmentId=true;
            }
            return isValidAppointmentId;
        }
        public async Task<bool> UpdateAppointment(Guid id, Appointment appointment)
        {
            if(id!=appointment.Id || appointment==null)
            {
                return false;
            }
            var appointmentToBeUpdated = AppointmentStore.AppointmentList.Find(appointment=>appointment.Id==id);
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