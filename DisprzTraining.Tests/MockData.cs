using Appointments;

namespace DisprzTraining.Tests
{
    public static class MockData
    {
        public static List<Appointment> TestData()
        {
          return new List<Appointment>()
        { 
          new Appointment
            {
              Id=1,
              AppointmentDate="2022-12-12",
              AppointmentDescription="test",
              AppointmentEndTime=DateTime.Now,
              AppointmentStartTime=DateTime.Today,
              AppointmentTitle="test"
            }
        };
        }
    }
}