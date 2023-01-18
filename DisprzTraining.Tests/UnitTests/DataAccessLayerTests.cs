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
        public void GetAppointments_WhenCalled_ReturnsList()
        {
            //Act
            var result = systemUnderTest.GetAppointments(new DateTime(2021, 10, 10, 10, 10, 10), new DateTime(2021, 10, 11, 10, 30, 30));
            //Assert
            Assert.IsType<List<Appointment>>(result);
            Assert.Equal(1, result.Count);
        }
        [Fact]
        public void GetAppointmentById_WhenCalled_ReturnsAppointment()
        {
            //Act
            var result = systemUnderTest.GetAppointmentById(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"));
            //Assert
            Assert.IsType<Appointment>(result);
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