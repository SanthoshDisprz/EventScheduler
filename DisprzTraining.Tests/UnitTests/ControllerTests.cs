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

namespace DisprzTraining.Tests.UnitTests
{
    public class ControllerTests
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
        public async Task GetAllAppointments_OnSuccess_ReturnsAllAppointments()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAllAppointments()).ReturnsAsync(new List<Appointment>());
            var SystemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await SystemUnderTest.GetAllAppointments() as OkObjectResult;
            var resultList = Assert.IsType<List<Appointment>>(okResult?.Value);
            
            //Assert
            Assert.Equal(0,resultList.Count);
        }


        [Fact]
        public async Task GetAppointmentsByDate_OnSuccess_ReturnsOkResult()
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
        public async Task GetAppointmentsByDate_OnSuccess_MatchResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments("2022-12-12")).ReturnsAsync(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.GetAppointments("2022-12-12");
            var ObjectResult=(OkObjectResult) okResult;
            //Assert
            ObjectResult.Value.Should().BeEquivalentTo(MockData.TestData());
        }
        [Fact]
        public async Task GetAppointmentById_OnSuccess_MatchResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointmentById(new Guid())).ReturnsAsync(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.GetAppointmentById(new Guid());
            var ObjectResult=(OkObjectResult) okResult;
            //Assert
            ObjectResult.Value.Should().BeEquivalentTo(MockData.TestData());
        }
        [Fact]
        public async Task GetAppointmentId_OnSuccess_ReturnsOkResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointmentById(new Guid())).ReturnsAsync(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.GetAppointmentById(new Guid()) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<List<Appointment>>(okResult.Value);
        }


        [Fact]
        public async Task AddAppointment_WhenNoAppointmentConflict_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(x=>x.AppointmentConflictCheck(testItem)).ReturnsAsync(false); 
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
            Mock.Setup(x=>x.AppointmentConflictCheck(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var conflictResult = await systemUnderTest.CreateAppointment(testItem);

            //Assert
            Assert.IsType<ConflictResult>(conflictResult);
        }


        [Fact]
        public async Task RemoveAppointment_WhenExistingIdPassed_ReturnsNoContentResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var systemUnderTest = new AppointmentsController(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(new Guid())).ReturnsAsync(true);
            //Act

            var noContentResult = await systemUnderTest.DeleteAppointment(new Guid());

            //Assert
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async Task RemoveAppointment_WhenNotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"))).ReturnsAsync(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            //Assert
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Fact]
        public async Task UpdateAppointment_ExistingIdPassed_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
             var testItem = new Appointment() {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = await systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem);
            //Assert
            Assert.IsType<OkResult>(okResult);
        }

        [Fact]
        public async Task UpdateAppointment_NotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new Appointment() {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).ReturnsAsync(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var notFoundResult = await systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem);
            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }
    }
}