using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Appointments;
using DisprzTraining.Models;

namespace DisprzTraining.Data
{
    public static class AppointmentsStore
    {
        public static List<Appointment> AppointmentList = new List<Appointment>(){
        new Appointment 
        {
            Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), 
            Description = "Daily updates about task", 
            Title = "Daily Sync-up", 
            StartTime=new DateTime(2099, 10, 10, 12, 10, 10), 
            EndTime=new DateTime(2099, 10, 10, 13, 10, 10) 
        },
        new Appointment 
        {
            Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), 
            Description = "Daily updates about task", 
            Title = "Daily Sync-up", 
            StartTime = new DateTime(2021, 10, 10, 10, 10, 10), 
            EndTime = new DateTime(2021, 10, 10, 12, 10, 10) 
        }, 
        new Appointment 
        {
            Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247471"), 
            Description = "Daily updates about task", 
            Title = "Daily Sync-up", 
            StartTime = new DateTime(2025, 08, 26, 05, 06, 07 ), 
            EndTime = new DateTime(2025, 08, 26, 06, 06, 07)
        },
        new Appointment
        {
            Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247361"), 
            Description = "Daily updates about task", 
            Title = "Daily Sync-up", 
            StartTime = new DateTime(2025, 09, 26, 05, 06, 07 ), 
            EndTime = new DateTime(2025, 09, 26, 06, 06, 07)
        }
        };
    }
}
