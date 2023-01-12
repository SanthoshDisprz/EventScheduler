using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DisprzTraining.Controllers;
using Moq;
using DisprzTraining.Business;
using Appointments;
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
        public void GetAllAppointments_WhenCalled_ReturnsList()
        {
            //Act
            var result = systemUnderTest.GetAllAppointments();
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public void GetAppointmentsByDate_WhenCalled_ReturnsList()
        {
            //Act
            var result = systemUnderTest.GetAppointmentsByDate(new DateTime(2022, 03, 03), timeZoneOffset);
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }


        [Fact]
        public void CreateAppointmentDAL_WhenCalled_ReturnsTrue()
        {
            //Arrange
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            //Act
            var result = systemUnderTest.CreateAppointment(testItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void DeleteAppointmentDAL_WhenCalled_ReturnsTrue()
        {
            //Arrange
            var testItem = new Appointment() { Id = new Guid(), Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            //Act
            var result = systemUnderTest.DeleteAppointment(testItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void UpdateAppointmentDAL_WhenCalled_ReturnsTrue()
        {
            //Arrange
            var testItem = new Appointment() { Id = new Guid(), Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            var UpdatedTestItem = new AddAppointment() { Description = "sss", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            //Act
            var result = systemUnderTest.UpdateAppointment(testItem, UpdatedTestItem);
            //Assert
            Assert.True(result);
        }

    }
}