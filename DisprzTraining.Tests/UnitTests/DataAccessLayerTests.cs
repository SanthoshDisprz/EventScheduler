using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DisprzTraining.Controllers;
using Moq;
using DisprzTraining.Business;
// using Appointments;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Data;
using FluentAssertions;
using DisprzTraining.DataAccess;
using DisprzTraining.Models;

namespace DisprzTraining.Tests.UnitTests
{
    public class DataAccessLayerTests
    {
        int timeZoneOffset = -330;
        //Tests for Data Access Layer
        AppointmentsDAL systemUnderTest = new AppointmentsDAL();


        [Fact]
        public void CreateGetUpdateAndDeleteAppointment_WhenCalled_ReturnsTrue()
        {
            //Creating an appointment

            //Arrange
            var testItem = new AddAppointment() { Description = "kkk", Title = "test", StartTime = new DateTime(2028, 08, 08, 01, 02, 03), EndTime = new DateTime(2028, 08, 08, 02, 02, 03) };
            //Act
            var result = systemUnderTest.CreateAppointment(testItem);
            //Assert
            Assert.True(result);

            //Getting the created appointment

            //Act
            var getResult = systemUnderTest.GetAppointments(new DateTime(2028, 08, 08, 01, 02, 03), new DateTime(2028, 08, 08, 02, 02, 03));
            //Assert
            Assert.IsType<List<Appointment>>(getResult);
            Assert.Equal(testItem.Title, getResult[0].Title);
            Assert.Equal(testItem.StartTime, getResult[0].StartTime);
            Assert.Equal(testItem.EndTime, getResult[0].EndTime);
            Assert.Equal(testItem.Description, getResult[0].Description);

            //Get appointments when start time passed as null returns empty list
            //Act
            var getResultWithStartTimeAsNull = systemUnderTest.GetAppointments(new DateTime(2029, 08, 08, 01, 02, 03), new DateTime(2029, 10, 11, 10, 30, 30));
            //Assert
            Assert.IsType<List<Appointment>>(getResultWithStartTimeAsNull);
            Assert.Equal(0, getResultWithStartTimeAsNull.Count);

            //Get appointments when end time passed as null returns empty list
            //Act
            var getResultWithEndTimeAsNull = systemUnderTest.GetAppointments(new DateTime(2027, 10, 11, 10, 10, 10), new DateTime(2027, 10, 11, 12, 12, 03));
            //Assert
            Assert.IsType<List<Appointment>>(getResultWithEndTimeAsNull);
            Assert.Equal(0, getResultWithEndTimeAsNull.Count);

            //Get appointments when both start time and end time as null returns empty list
            //Act
            var getResultWithNull = systemUnderTest.GetAppointments(new DateTime(2030, 08, 08, 01, 02, 03), new DateTime(2020, 08, 08, 02, 02, 03));
            //Assert
            Assert.IsType<List<Appointment>>(getResultWithNull);
            Assert.Equal(0, getResultWithNull.Count);

            //Getting the appointment by title
            //Act
            var getByTitleResult = systemUnderTest.GetAppointmentsByTitle("test");
            //Assert
            Assert.IsType<List<Appointment>>(getByTitleResult);
            Assert.Equal(testItem.Title, getByTitleResult[0].Title);

            //Updating the appointment

            //Act
            var existingAppointment = new Appointment() { Id = getResult[0].Id, Title = getResult[0].Title, StartTime = getResult[0].StartTime, EndTime = getResult[0].EndTime, Description = getResult[0].Description };
            var updatedAppointmentTestItem = new AddAppointment() { Description = "kkk", Title = "updated", StartTime = new DateTime(2028, 08, 08, 01, 02, 03), EndTime = new DateTime(2028, 08, 08, 02, 02, 03) };
            var updateResult = systemUnderTest.UpdateAppointment(existingAppointment, updatedAppointmentTestItem);
            //Assert
            Assert.True(updateResult);

            //getting the appointment by Id
            //Act
            var getAppointmentById = systemUnderTest.GetAppointmentById(getResult[0].Id);
            //Assert
            Assert.IsType<Appointment>(getAppointmentById);

            //get all appointments
            var allAppointments = systemUnderTest.GetAllAppointments();
            Assert.IsType<List<Appointment>>(allAppointments);
            Assert.Equal(5, allAppointments.Count);

            //Deleting the appointment
            var deleteAppointment = systemUnderTest.DeleteAppointment(getAppointmentById);
            Assert.True(deleteAppointment);

        }


    }
}