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

namespace DisprzTraining.Tests.UnitTests
{
    public class DataAccessLayerTests
    {
         //Tests for Data Access Layer
        AppointmentDAL systemUnderTest = new AppointmentDAL();

        [Fact]
        public async Task GetAllAppointments_WhenCalled_ReturnsList()
        {
            //Act
            var result = await systemUnderTest.GetAllAppointments();
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public async Task GetAppointmentsByDate_WhenCalled_ReturnsList()
        {
            //Act
            var result = await systemUnderTest.GetAppointments("2022-03-03");
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public async Task GetAppointmentById_WhenCalled_ReturnsList()
        {
            //Act
            var result = await systemUnderTest.GetAppointmentById(new Guid());
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }

        [Fact]
        public async Task CreateAppointmentDAL_WhenCalled_ReturnsTrue()
        {
            //Arrange
            var testItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            //Act
            var result = await systemUnderTest.CreateAppointment(testItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task DeleteAppointmentDAL_WhenCalled_ReturnsTrue()
        {
            //Arrange
            var testItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            //Act
            var result = await systemUnderTest.DeleteAppointment(testItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task UpdateAppointmentDAL_WhenCalled_ReturnsTrue()
        {
            //Arrange
            var testItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            var UpdatedTestItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "sss", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            //Act
            var result = await systemUnderTest.UpdateAppointment(testItem, UpdatedTestItem);
            //Assert
            Assert.True(result);
        }

    }
}