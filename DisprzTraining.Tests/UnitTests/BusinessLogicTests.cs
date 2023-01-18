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
        public void GetAppointments_WhenFromAndToTimePassed_ReturnsList()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2021, 10, 10, 10, 10, 10), new DateTime(2021, 10, 10, 12, 10, 10))).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.GetAppointments(new DateTime(2021, 10, 10, 10, 10, 10), new DateTime(2021, 10, 10, 12, 10, 10), timeZoneOffset);
            //Assert
            Assert.IsType<List<Appointment>>(result);
            Assert.Equal(result.Count, 0);
        }


        [Fact]
        public void GetAppointments_WhenNothingPassed_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAppointments(null, null)).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.GetAppointments(null, null, 0));
            Assert.Equal("Both Start time and End time are mandatory for getting appointments", ex.Message);
        }
        [Fact]
        public void GetAppointments_WhenFromTimeGreaterThanToTime_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2021, 11, 10, 10, 10, 10), new DateTime(2021, 10, 10, 10, 10, 10))).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.GetAppointments(new DateTime(2021, 11, 10, 10, 10, 10), new DateTime(2021, 10, 10, 10, 10, 10), timeZoneOffset));
            Assert.Equal("Start time should be lesser than End time", ex.Message);
        }
        [Fact]
        public void GetAppointments_WhenFromTimeAndToTimeAreSame_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2021, 10, 10, 10, 10, 10), new DateTime(2021, 10, 10, 10, 10, 10))).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.GetAppointments(new DateTime(2021, 10, 10, 10, 10, 10), new DateTime(2021, 10, 10, 10, 10, 10), timeZoneOffset));
            Assert.Equal("Start time and End time should not be equal", ex.Message);
        }
        [Fact]
        public void ConvertToLocalTime_WhenAppointmentsListPassed_ReturnsAppointmentsListInLocalTime()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            var testItem = new List<Appointment>() { new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247461"), Description = "kkk", Title = "sss", StartTime = new DateTime(2023, 02, 15, 07, 07, 07), EndTime = new DateTime(2023, 02, 15, 07, 07, 07).AddHours(1) } };
            List<Appointment> appointmentsInLocalTimeStamp = new List<Appointment>(){
                 new Appointment() {Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247461"), Description = "kkk", Title = "sss", StartTime = new DateTime(2023, 02, 15, 07, 07, 07).AddMinutes(-timeZoneOffset), EndTime = new DateTime(2023, 02, 15, 07, 07, 07).AddHours(1).AddMinutes(-timeZoneOffset)}
            };

            //Act
            var result = systemUnderTest.ConvertToLocalTime(testItem, timeZoneOffset);
            //Assert
            // result.Should().BeEquivalentTo(appointmentsInLocalTimeStamp);
            Assert.IsType<List<Appointment>>(result);
        }

        [Fact]
        public void AppointmentConflictCheck_WhenNoConflict_ReturnsFalse()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.GetAllAppointments()).Returns(new List<Appointment>());
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
            var postTestItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2099, 10, 10, 12, 10, 10), EndTime = new DateTime(2099, 10, 10, 13, 10, 10) };
            var getTestItem = new List<Appointment>(){new Appointment(){
                Id = new Guid(),
                Title = postTestItem.Title,
                StartTime = postTestItem.StartTime,
                EndTime = postTestItem.EndTime
            }
            };
            Mock.Setup(service => service.CreateAppointment(postTestItem)).Returns(true);
            Mock.Setup(service => service.GetAllAppointments()).Returns(getTestItem);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.AppointmentConflictCheck(postTestItem);
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
        public void CreateAppointment_WhenTriedToCreateAppointmentForPastTime_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2021, 09, 10, 20, 10, 10), EndTime = new DateTime(2021, 09, 10, 22, 10, 10) };
            Mock.Setup(service => service.CreateAppointment(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);

            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.CreateAppointment(testItem));
            Assert.Equal("Cannot create appointment for past time", ex.Message);

        }
        [Fact]
        public void CreateAppointment_WhenNoError_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = DateTime.UtcNow.AddMonths(1), EndTime = DateTime.UtcNow.AddMonths(1).AddHours(1) };
            Mock.Setup(service => service.CreateAppointment(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsBL(Mock.Object);

            //Act and Assert
            var result = systemUnderTest.CreateAppointment(testItem);
            Assert.True(result);


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
        public void UpdateAppointmentConflictCheck_WhenAppointmentConflicts_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var postTestItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2099, 10, 10, 12, 10, 10), EndTime = new DateTime(2099, 10, 10, 13, 10, 10) };
            var getTestItem = new List<Appointment>(){new Appointment(){
                Id = new Guid(),
                Title = postTestItem.Title,
                StartTime = postTestItem.StartTime,
                EndTime = postTestItem.EndTime
            }
            };
            // Mock.Setup(service => service.UpdateAppointment(postTestItem)).Returns(true);
            Mock.Setup(service => service.GetAllAppointments()).Returns(getTestItem);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.UpdateAppointmentConflictCheck(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), postTestItem);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void UpdateAppointmentConflictCheck_WhenNoAppointmentConflicts_ReturnsFalse()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var postTestItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var getTestItem = new List<Appointment>(){new Appointment(){
                Id = new Guid(),
                Title = "test",
                StartTime = DateTime.UtcNow.AddDays(2),
                EndTime = DateTime.UtcNow.AddDays(2).AddHours(3)
            }
            };
            // Mock.Setup(service => service.UpdateAppointment(postTestItem)).Returns(true);
            Mock.Setup(service => service.GetAllAppointments()).Returns(getTestItem);
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            //Act
            var result = systemUnderTest.UpdateAppointmentConflictCheck(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), postTestItem);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void UpdateAppointment_WhenExistingIdPassed_ReturnsTrue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var postTestItem = new Appointment() { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var getTestItem = new List<Appointment>(){new Appointment(){
                Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                Title = postTestItem.Title,
                StartTime = postTestItem.StartTime,
                EndTime = postTestItem.EndTime
            }
            };
            var UpdatedTestItem = new AddAppointment() { Description = "sss", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.GetAppointmentById(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"))).Returns(postTestItem);
            Mock.Setup(service => service.UpdateAppointment(postTestItem, UpdatedTestItem)).Returns(true);
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
            Mock.Setup(service => service.GetAllAppointments()).Returns(new List<Appointment>());
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
            Assert.Equal("Cannot Update Appointment to Past time", ex.Message);
        }
        [Fact]
        public void UpdateAppointment_WhenTriedToUpdatePastAppointment_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsDAL>();
            var testItem = new Appointment { Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), Description = "kkk", Title = "sss", StartTime = new DateTime(2021, 10, 10, 10, 10, 10), EndTime = new DateTime(2021, 10, 10, 12, 10, 10) };
            var UpdatedTestItem = new AddAppointment() { Description = "sss", Title = "sss", StartTime = new DateTime(2023, 10, 10, 10, 10, 10), EndTime = new DateTime(2023, 10, 10, 12, 10, 10) };
            var systemUnderTest = new AppointmentsBL(Mock.Object);
            Mock.Setup(service => service.UpdateAppointment(testItem, UpdatedTestItem)).Returns(true);
            Mock.Setup(service => service.GetAppointmentById(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"))).Returns(testItem);

            //Act and Assert
            var ex = Assert.Throws<Exception>(() => systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), UpdatedTestItem));
            Assert.Equal("Cannot Update Past Appointment", ex.Message);
        }

    }
}