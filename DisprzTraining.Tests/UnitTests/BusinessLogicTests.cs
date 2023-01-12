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
    public class BusinessLogicTests
    {
        //Tests for Business Logic
        int timeZoneOffset = -330;

        [Fact]
        public void GetAppointments_WhenDatePassed_ReturnsList()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAppointmentsByDate(new DateTime(2022, 07, 06), timeZoneOffset)).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.GetAppointments(new DateTime(2022, 07, 06), timeZoneOffset, null);
            //Assert
            Assert.IsType<List<Appointment>>(result);
            Assert.Equal(result.Count, 0);
        }

        [Fact]
        public void GetAppointments_WhenDurationPassed_ReturnsList()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAllAppointments()).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.GetAppointments(null, null, "Month");
            //Assert
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public void GetAppointments_WhenNullPassed_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAllAppointments()).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.GetAppointments(null, null, null));
            Assert.Equal("Either Appointment Date or Duration is mandatory", ex.Message);
        }
        [Fact]
        public void GetAppointments_WhenInvalidDurationPassed_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAllAppointments()).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.GetAppointments(null, null, "test"));
            Assert.Equal("Enter valid Duration", ex.Message);
        }

        [Fact]
        public void AppointmentConflictCheck_WhenNoConflict_ReturnsFalse()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.CreateAppointment(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.AppointmentConflictCheck(testItem);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void AppointmentConflictCheck_WhenAppointmentConflicts_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2099, 10, 10, 12, 10, 10), EndTime = new DateTime(2099, 10, 10, 13, 10, 10) };
            Mock.Setup(service => service.CreateAppointment(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.AppointmentConflictCheck(testItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void CreateAppointment_WhenAppointmentStartTimeGreaterThanEndTime_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = new DateTime(2022, 09, 10, 20, 10, 10) };
            Mock.Setup(service => service.CreateAppointment(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);

            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.CreateAppointment(testItem));
            Assert.Equal("Appointment Start time should be greater than End time", ex.Message);


        }
        [Fact]
        public void CreateAppointment_WhenAppointmentStartTimeAndEndTimeAreSame_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2022, 09, 10, 20, 10, 10), EndTime = new DateTime(2022, 09, 10, 20, 10, 10) };
            Mock.Setup(service => service.CreateAppointment(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);

            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.CreateAppointment(testItem));
            Assert.Equal("Appointment Start time and End time should not be same", ex.Message);

        }




        [Fact]
        public void DeleteAppointment_WhenExistingIdPassed_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), Description = "kkk", Title = "sss", StartTime = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(2) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(testItem)).Returns(true);
            //Act

            var result = systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void DeleteAppointment_WhenNotExistingIdPassed_ReturnsFalse()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247480"), Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(testItem)).Returns(true);
            //Act

            var result = systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247480"));

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void DeleteAppointment_WhenTriedToDeleteOlderDateTimeAppointment_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), Description = "kkk", Title = "sss", StartTime = new DateTime(2021, 10, 10, 10, 10, 10), EndTime = new DateTime(2021, 10, 10, 12, 10, 10) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(testItem)).Returns(true);


            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481")));
            Assert.Equal("Cannot Delete Older Appointments", ex.Message);
        }
        [Fact]
        public void UpdateAppointment_WhenExistingIdPassed_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var UpdatedTestItem = new AddAppointment() { Description = "sss", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.UpdateAppointment(testItem, UpdatedTestItem)).Returns(true);
            //Act

            var result = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), UpdatedTestItem);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void UpdateAppointment_WhenNotExistingIdPassed_ReturnsFalse()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var UpdatedTestItem = new AddAppointment() { Description = "sss", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.UpdateAppointment(testItem, UpdatedTestItem)).Returns(true);
            //Act

            var result = systemUnderTest.UpdateAppointment(new Guid(), UpdatedTestItem);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void UpdateAppointment_WhenOlderDateTimePassed_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var UpdatedTestItem = new AddAppointment() { Description = "sss", Title = "sss", StartTime = new DateTime(2021, 10, 10, 10, 10, 10), EndTime = new DateTime(2021, 10, 10, 12, 10, 10) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.UpdateAppointment(testItem, UpdatedTestItem)).Returns(true);

            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), UpdatedTestItem));
            Assert.Equal("Cannot Update Older Appointments", ex.Message);
        }

    }
}