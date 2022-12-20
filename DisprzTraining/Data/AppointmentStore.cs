using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointments;

namespace DisprzTraining.Data
{
    public static class AppointmentStore
    {
        public static List<Appointment> AppointmentList = new List<Appointment>(){new Appointment {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime=new DateTime(2022, 09, 10, 10, 10, 10), AppointmentEndTime=new DateTime(2022, 09, 10, 20, 10, 10) }};

    }
}
