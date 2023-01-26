// using Appointments;
using DisprzTraining.Models;
using System;

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
              Description="test",
              EndTime=new DateTime(2022, 10, 10, 10, 10, 10),
              StartTime=DateTime.Today,
              Title="test"
            }
        };
        }
    }
}