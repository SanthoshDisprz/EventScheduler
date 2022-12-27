using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointments;

namespace DisprzTraining.Data
{
    public static class AppointmentStore
    {
        public static List<Appointment> AppointmentList = new List<Appointment>(){new Appointment {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartDateTime=new DateTime(2099, 10, 10, 12, 10, 10), AppointmentEndDateTime=new DateTime(2099, 10, 10, 13, 10, 10) },
        new Appointment {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), AppointmentDate = "2021-10-10", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartDateTime = new DateTime(2021, 10, 10, 10, 10, 10), AppointmentEndDateTime = new DateTime(2021, 10, 10, 12, 10, 10) }};

    }
}
