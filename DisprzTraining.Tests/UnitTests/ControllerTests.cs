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
using DisprzTraining.Models;

namespace DisprzTraining.Tests.UnitTests
{
    public class ControllerTests
    {
   int timeZoneOffset = -330;
        [Fact]
        public void GetAppointments_OnSuccess_ReturnsAllAppointments()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2023, 01, 04),timeZoneOffset, null)).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointments(new DateTime(2023, 01, 04),timeZoneOffset, null) as OkObjectResult;
            var resultList = Assert.IsType<List<Appointment>>(okResult?.Value);
            
            //Assert
            Assert.Equal(0,resultList.Count);
        }


        [Fact]
        public void GetAppointments_WhenDatePassed_ReturnsOkResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2022, 12, 12),timeZoneOffset, null)).Returns(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointments(new DateTime(2022, 12, 12),timeZoneOffset, null) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<List<Appointment>>(okResult.Value);
        }


        [Fact]
        public void GetAppointmentsWhenDatePassed_OnSuccess_MatchResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2023, 01, 04),timeZoneOffset, null)).Returns(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointments(new DateTime(2023, 01, 04),timeZoneOffset, null);
            var ObjectResult=(OkObjectResult) okResult;
            //Assert
            ObjectResult.Value.Should().BeEquivalentTo(MockData.TestData());
        }

        [Fact]
        public void GetAppointments_WhenDurationPassed_ReturnsOkResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(null,null, "Month")).Returns(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointments(null,null, "Month") as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<List<Appointment>>(okResult.Value);
        }
        [Fact]
        public void GetAppointments_WhenNullPassed_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(null,null, null)).Throws(new Exception("Either Date or Duration is Mandatory"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.GetAppointments(null,null, null);

            //Assert
             var badRequestResult=(BadRequestObjectResult) exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse(){StatusCode=400, ErrorMessage="Either Date or Duration is Mandatory"});
        }
       


        [Fact]
        public void CreateAppointment_WhenNoAppointmentConflict_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(x=>x.AppointmentConflictCheck(testItem)).Returns(false); 
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var createdResult = systemUnderTest.CreateAppointment(testItem);

            //Assert
            Assert.IsType<CreatedResult>(createdResult);
        }

        [Fact]
        public void CreateAppointment_WhenAppointmentsConflict_ReturnsConflict()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(x=>x.AppointmentConflictCheck(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var conflictResult = systemUnderTest.CreateAppointment(testItem);

            //Assert
            Assert.IsType<ConflictObjectResult>(conflictResult);
        }
        [Fact]
        public void CreateAppointment_WhenAppointmentStartTimeAndEndTimeAreSame_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2022,12,12,12,12,12), EndTime = new DateTime(2022,12,12,12,12,12) };
            Mock.Setup(x=>x.CreateAppointment(testItem)).Throws(new Exception("Appointment Start time and End time should not be same"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult =  systemUnderTest.CreateAppointment(testItem);
            var badRequestResult=(BadRequestObjectResult) exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse(){StatusCode=400, ErrorMessage="Appointment Start time and End time should not be same"});
        }
        [Fact]
        public void CreateAppointment_WhenAppointmentStartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2022,12,12,12,12,12), EndTime = new DateTime(2022,12,12,12,12,12) };
            Mock.Setup(x=>x.CreateAppointment(testItem)).Throws(new Exception("Appointment Start time should be greater than End time"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult =  systemUnderTest.CreateAppointment(testItem);
            var badRequestResult=(BadRequestObjectResult) exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse(){StatusCode=400, ErrorMessage="Appointment Start time should be greater than End time"});
        }

        


        [Fact]
        public void DeleteAppointment_WhenExistingIdPassed_ReturnsNoContentResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var systemUnderTest = new AppointmentsController(Mock.Object);
            Mock.Setup(service => service.DeleteAppointment(new Guid())).Returns(true);
            //Act

            var noContentResult = systemUnderTest.DeleteAppointment(new Guid());

            //Assert
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public void DeleteAppointment_WhenNotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"))).Returns(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            //Assert
            Assert.IsType<NotFoundObjectResult>(okResult);
        }
        [Fact]
        public void DeleteAppointment_WhenTriedToDeleteOlderAppointments_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"))).Throws(new Exception("Cannot Delete Older Appointments"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"));
            var badRequestResult=(BadRequestObjectResult) exceptionResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(exceptionResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse(){StatusCode=400, ErrorMessage="Cannot Delete Older Appointments"});
        }

        [Fact]
        public void UpdateAppointment_ExistingIdPassed_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
             var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).Returns(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem);
            //Assert
            Assert.IsType<OkResult>(okResult);
        }

        [Fact]
        public void UpdateAppointment_NotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).Returns(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var notFoundResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem);
            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }
        [Fact]
        public void UpdateAppointment_WhenTriedToUpdateOlderAppointment_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem)).Throws(new Exception("Cannot Update Older Appointment"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem);
            var badRequestResult=(BadRequestObjectResult) exceptionResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(exceptionResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse(){StatusCode=400, ErrorMessage="Cannot Update Older Appointment"});
        }
         [Fact]
        public void UpdateAppointment_WhenAppointmentStartTimeAndEndTimeAreSame_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2022,12,12,12,12,12), EndTime = new DateTime(2022,12,12,12,12,12) };
            Mock.Setup(x=>x.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem)).Throws(new Exception("Appointment Start time and End time should not be same"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem);
            var badRequestResult=(BadRequestObjectResult) exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse(){StatusCode=400, ErrorMessage="Appointment Start time and End time should not be same"});
        }
        [Fact]
        public void UpdateAppointment_WhenAppointmentStartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "kkk", Title = "sss", StartTime = new DateTime(2022,12,12,12,12,12), EndTime = new DateTime(2022,12,12,12,12,12) };
            Mock.Setup(x=>x.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem)).Throws(new Exception("Appointment Start time should be greater than End time"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem);
            var badRequestResult=(BadRequestObjectResult) exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse(){StatusCode=400, ErrorMessage="Appointment Start time should be greater than End time"});
        }
    }
}