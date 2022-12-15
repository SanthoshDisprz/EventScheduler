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

namespace DisprzTraining.Tests
{
    public class AppointmentServiceTest
    {
        [Fact]
        public async Task GetAllAppointments_OnSuccess_ReturnsOkResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAllAppointments()).ReturnsAsync(new List<Appointment>());
            var SystemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await SystemUnderTest.GetAllAppointments() as OkObjectResult;
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<List<Appointment>>(okResult.Value);
        }


        [Fact]
        public async Task GetAppointments_OnSuccess_ReturnsOkResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments("2022-12-12")).ReturnsAsync(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.GetAppointments("2022-12-12") as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<List<Appointment>>(okResult.Value);
        }

        [Fact]
        public async Task GetAppointments_WhenNoAppointments_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments("2022-12-12")).ReturnsAsync(new List<Appointment>());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var notFoundResult = await systemUnderTest.GetAppointments("2022-12-12");

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }


        [Fact]
        public async Task AddAppointment_WhenNoAppointmentConflict_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var createdResult = await systemUnderTest.CreateAppointment(testItem);

            //Assert
            Assert.IsType<CreatedResult>(createdResult);
        }

        [Fact]
        public async Task AddAppointment_WhenAppointmentsConflict_ReturnsConflict()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var conflictResult = await systemUnderTest.CreateAppointment(testItem);

            //Assert
            Assert.IsType<ConflictResult>(conflictResult);
        }

        [Fact]
        public async Task RemoveAppointment_WhenExistingIdPassed_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var systemUnderTest = new AppointmentsController(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(1)).ReturnsAsync(true);
            //Act

            var okResult = await systemUnderTest.DeleteAppointment(1);

            //Assert
            Assert.IsType<OkResult>(okResult);
        }

        [Fact]
        public async Task RemoveAppointment_WhenNotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.DeleteAppointment(10)).ReturnsAsync(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.DeleteAppointment(10);
            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public async Task UpdateAppointment_ExistingIdPassed_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
             var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(1, testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.UpdateAppointment(1, testItem);
            //Assert
            Assert.IsType<OkResult>(okResult);
        }

        [Fact]
        public async Task UpdateAppointment_NotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(1, testItem)).ReturnsAsync(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var notFoundResult = await systemUnderTest.UpdateAppointment(1, testItem);
            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }




    }
}