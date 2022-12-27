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
              Id=new Guid(),
              AppointmentDate="2022-12-12",
              AppointmentDescription="test",
              AppointmentEndDateTime=new DateTime(2022, 10, 10, 10, 10, 10),
              AppointmentStartDateTime=DateTime.Today,
              AppointmentTitle="test"
            }
        };
        }
    }
}