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
    public class BusinessLogicTests
    {
        //Tests for Business Logic

        [Fact]
        public async Task GetAllAppointmentsBL_WhenInvoked_ReturnsList()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            Mock.Setup(service => service.GetAllAppointments()).ReturnsAsync(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = await systemUnderTest.GetAllAppointments();
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public async Task GetAppointmentsByDate_WhenValidDatePassed_ReturnsList()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            Mock.Setup(service => service.GetAppointments("2022-07-06")).ReturnsAsync(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = await systemUnderTest.GetAppointments("2022-07-06");
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public async Task GetAppointmentsByDate_WhenInValidDatePassed_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            Mock.Setup(service => service.GetAppointments("20220706")).ReturnsAsync(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = async () =>{await systemUnderTest.GetAppointments("20220706");};
            //Assert
           await Assert.ThrowsAsync<Exception>(result);
           await result.Should().ThrowAsync<Exception>().WithMessage("Invalid Input Type for Appointment Date. Appointment Date should be of Date format");
            
        }
        [Fact]
        public async Task GetAppointmentById_WhenIdPassed_ReturnsList()
        {
            //Arrange
             var Mock = new Mock<IAppointmentDAL>();
            Mock.Setup(service => service.GetAppointmentById(new Guid())).ReturnsAsync(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = await systemUnderTest.GetAppointmentById(new Guid());
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public async Task AppointmentConflictCheck_WhenNoConflict_ReturnsFalse()
        {
            //Arrange
             var Mock = new Mock<IAppointmentDAL>();
             var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = await systemUnderTest.AppointmentConflictCheck(testItem);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public async Task AppointmentConflictCheck_WhenAppointmentConflicts_ReturnsTrue()
        {
            //Arrange
             var Mock = new Mock<IAppointmentDAL>();
             var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime=new DateTime(2022, 09, 10, 10, 10, 10), AppointmentEndTime=new DateTime(2022, 09, 10, 20, 10, 10) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = await systemUnderTest.AppointmentConflictCheck(testItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task AppointmentConflictCheck_WhenAppointmentStartTimeGreaterThanEndTime_ThrowsException()
        {
            //Arrange
             var Mock = new Mock<IAppointmentDAL>();
             var testItem = new Appointment() { AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime=DateTime.Now, AppointmentEndTime=new DateTime(2022, 09, 10, 20, 10, 10) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = async () =>{await systemUnderTest.AppointmentConflictCheck(testItem);};
            //Assert
           await Assert.ThrowsAsync<Exception>(result);
           await result.Should().ThrowAsync<Exception>().WithMessage("Appointment Start time should be greater than End time");
            
        }
        [Fact]
        public async Task AppointmentConflictCheck_AppointmentStartTimeAndEndTimeIsSame_ThrowsException()
        {
            //Arrange
             var Mock = new Mock<IAppointmentDAL>();
             var testItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime=new DateTime(2022, 09, 10, 20, 10, 10), AppointmentEndTime=new DateTime(2022, 09, 10, 20, 10, 10) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = async () =>{await systemUnderTest.AppointmentConflictCheck(testItem);};
            //Assert
           await Assert.ThrowsAsync<Exception>(result);
           await result.Should().ThrowAsync<Exception>().WithMessage("Appointment Start time and End time should not be same");
            
        }
        
        [Fact]
        public async Task CreateAppointment_WhenValidDatePassed_ReturnsTrue()
        {
            //Arrange
             var Mock = new Mock<IAppointmentDAL>();
             var testItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = await systemUnderTest.CreateAppointment(testItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task CreateAppointment_WhenInValidDatePassed_ThrowsException()
        {
            //Arrange
             var Mock = new Mock<IAppointmentDAL>();
             var testItem = new Appointment() { AppointmentDate = "20220309", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.CreateAppointment(testItem)).ReturnsAsync(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result =async()=> {await systemUnderTest.CreateAppointment(testItem);};
            //Assert
            await Assert.ThrowsAsync<Exception>(result);
            await result.Should().ThrowAsync<Exception>().WithMessage("Invalid Input Type for Appointment Date. Appointment Date should be of Date format");
        }
        

        [Fact]
        public async Task DeleteAppointment_WhenExistingIdPassed_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            var testItem = new Appointment() {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(testItem)).ReturnsAsync(true);
            //Act

            var result = await systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task DeleteAppointment_WhenNotExistingIdPassed_ReturnsFalse()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            var testItem = new Appointment() {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = new DateTime().ToLocalTime(), AppointmentEndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(testItem)).ReturnsAsync(true);
            //Act

            var result = await systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"));

            //Assert
            Assert.False(result);
        }
        [Fact]
         public async Task UpdateAppointment_WhenExistingIdPassed_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            var testItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };
            var UpdatedTestItem = new Appointment() {Id=new Guid(), AppointmentDate = "2022-03-09", AppointmentDescription = "sss", AppointmentTitle = "sss", AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.UpdateAppointment(testItem,UpdatedTestItem)).ReturnsAsync(true);
            //Act

            var result = await systemUnderTest.UpdateAppointment(new Guid(), testItem);

            //Assert
            Assert.True(result);
        }
        [Fact]
         public async Task UpdateAppointment_WhenNotExistingIdPassed_ReturnsFalse()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            var testItem = new Appointment() {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), AppointmentDate = "2022-03-09", AppointmentDescription = "kkk", AppointmentTitle = "sss", AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };
            var UpdatedTestItem = new Appointment() {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), AppointmentDate = "2022-03-09", AppointmentDescription = "sss", AppointmentTitle = "sss", AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.UpdateAppointment(testItem,UpdatedTestItem)).ReturnsAsync(true);
            //Act

            var result = await systemUnderTest.UpdateAppointment(new Guid(), testItem);

            //Assert
            Assert.False(result);
        }

    }
}