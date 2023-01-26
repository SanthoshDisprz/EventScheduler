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
using DisprzTraining.Models;

namespace DisprzTraining.Tests.UnitTests
{
    //Controller Test cases
    public class ControllerTests
    {
        int timeZoneOffset = -330;
        //Get Appointments - Passing test cases
        [Fact]
        public void GetAppointments_WhenFromAndToTimePassed_ReturnsAllAppointments()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2023, 01, 04), new DateTime(2023, 01, 05), timeZoneOffset)).Returns(new List<Appointment>());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointments(new GetAppointmentsQueryParameters(){From = new DateTime(2023, 01, 04),To = new DateTime(2023, 01, 05),TimeZoneOffset = timeZoneOffset}) as OkObjectResult;
            var resultList = Assert.IsType<List<Appointment>>(okResult?.Value);

            //Assert
            Assert.Equal(0, resultList.Count);
        }


        [Fact]
        public void GetAppointments_WhenFromAndToTimePassed_ReturnsOkResultAndCorrectReturnTypeAndValue()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2023, 01, 04), new DateTime(2023, 01, 05), timeZoneOffset)).Returns(MockData.TestData());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointments(new GetAppointmentsQueryParameters(){From = new DateTime(2023, 01, 04),To = new DateTime(2023, 01, 05), TimeZoneOffset = timeZoneOffset}) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<List<Appointment>>(okResult.Value);
            okResult.Value.Should().BeEquivalentTo(MockData.TestData());
        }

        //Get Appointments - Failing test cases

        [Fact]
        public void GetAppointments_WhenNothingPassed_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(null, null, 0)).Throws(new Exception("Both Start time and End time are mandatory for getting appointments"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var badRequestResult = systemUnderTest.GetAppointments(new GetAppointmentsQueryParameters(){ From = null, To = null, TimeZoneOffset = 0});
            var badRequestObjectResult = (BadRequestObjectResult)badRequestResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestObjectResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Both Start time and End time are mandatory for getting appointments" });
        }
        [Fact]
        public void GetAppointments_WhenFromAndToTimeAreSame_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2023, 01, 04), new DateTime(2023, 01, 04), 0)).Throws(new Exception("Start time and End time should not be equal"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var badRequestResult = systemUnderTest.GetAppointments(new GetAppointmentsQueryParameters(){From = new DateTime(2023, 01, 04), To = new DateTime(2023, 01, 04), TimeZoneOffset = 0});
            var badRequestObjectResult = (BadRequestObjectResult)badRequestResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestObjectResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Start time and End time should not be equal" });
        }
        [Fact]
        public void GetAppointments_WhenFromTimeGreaterThanToTime_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.GetAppointments(new DateTime(2023, 01, 06), new DateTime(2023, 01, 04), 0)).Throws(new Exception("Start time should be lesser than End time"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var badRequestResult = systemUnderTest.GetAppointments(new GetAppointmentsQueryParameters(){From = new DateTime(2023, 01, 06),To = new DateTime(2023, 01, 04),TimeZoneOffset = 0});
            var badRequestObjectResult = (BadRequestObjectResult)badRequestResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestObjectResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Start time should be lesser than End time" });
        }

        //Create appointment - Passing Test cases


        [Fact]
        public void CreateAppointment_WhenNoAppointmentConflict_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1), GuestsList=new List<string>(){"santhoshoct31@gmail.com"} };
            Mock.Setup(x => x.CreateAppointment(testItem)).Returns(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var createdResult = systemUnderTest.CreateAppointment(testItem);

            //Assert
            Assert.IsType<CreatedResult>(createdResult);
        }

        //Create appointment - Failing Test cases

        [Fact]
        public void CreateAppointment_WhenAppointmentsConflict_ReturnsConflict()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(x => x.CreateAppointment(testItem)).Returns(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var conflictResult = systemUnderTest.CreateAppointment(testItem);
            var conflictRequestResult = (ConflictObjectResult)conflictResult;

            //Assert
            Assert.IsType<ConflictObjectResult>(conflictResult);
            conflictRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 409, ErrorMessage = "Appointment Conflict Occured." });
        }
        [Fact]
        public void CreateAppointment_WhenAppointmentStartTimeAndEndTimeAreSame_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime(2022, 12, 12, 12, 12, 12), EndTime = new DateTime(2022, 12, 12, 12, 12, 12) };
            Mock.Setup(x => x.CreateAppointment(testItem)).Throws(new Exception("Appointment Start time and End time should not be same"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.CreateAppointment(testItem);
            var badRequestResult = (BadRequestObjectResult)exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Appointment Start time and End time should not be same" });
        }
        [Fact]
        public void CreateAppointment_WhenAppointmentStartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime(2022, 12, 12, 12, 12, 12), EndTime = new DateTime(2022, 12, 12, 12, 12, 12) };
            Mock.Setup(x => x.CreateAppointment(testItem)).Throws(new Exception("End time should be greater than Start time"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.CreateAppointment(testItem);
            var badRequestResult = (BadRequestObjectResult)exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "End time should be greater than Start time" });
        }
        [Fact]
        public void CreateAppointment_WhenTriedToCreateAppointmentForPastTime_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime(2021, 11, 12, 12, 12, 12), EndTime = new DateTime(2021, 12, 12, 12, 12, 12) };
            Mock.Setup(x => x.CreateAppointment(testItem)).Throws(new Exception("Cannot create appointment for past time"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.CreateAppointment(testItem);
            var badRequestResult = (BadRequestObjectResult)exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Cannot create appointment for past time" });
        }

        //Delete appointment - Passing test case
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

        //Delete appointment - Failing test cases

        [Fact]
        public void DeleteAppointment_WhenNotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(service => service.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"))).Returns(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var notFoundResult = systemUnderTest.DeleteAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            var notFoundObjectResult = (NotFoundObjectResult)notFoundResult;
            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
            notFoundObjectResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 404, ErrorMessage = "The given Id is not found" });
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
            var badRequestResult = (BadRequestObjectResult)exceptionResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(exceptionResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Cannot Delete Older Appointments" });
        }
        //Update appointment - Passing test case
        [Fact]
        public void UpdateAppointment_ExistingIdPassedAndNoConflict_ReturnsOk()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1), GuestsList=new List<string>(){"santhoshoct31@gmail.com"} };
            Mock.Setup(service => service.UpdateAppointmentConflictCheck(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).Returns(false);
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).Returns(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem);
            //Assert
            Assert.IsType<OkResult>(okResult);
        }

        //Update appointment - Failing test cases
        [Fact]
        public void UpdateAppointment_ExistingIdPassedAndAppointmentConflict_ReturnsConflict()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime(2021, 10, 10, 10, 10, 10), EndTime = new DateTime(2021, 10, 10, 12, 10, 10) };
            Mock.Setup(service => service.UpdateAppointmentConflictCheck(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).Returns(true);
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).Returns(true);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var conflictResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem);
            var conflictObjectResult = (ConflictObjectResult)conflictResult;
            //Assert
            Assert.IsType<ConflictObjectResult>(conflictResult);
            conflictObjectResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 409, ErrorMessage = "Appointment Conflict Occured." });
            
        }

        [Fact]
        public void UpdateAppointment_NotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime().ToLocalTime(), EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem)).Returns(false);
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var notFoundResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), testItem);
            var notFoundObjectResult = (NotFoundObjectResult)notFoundResult;
            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
            notFoundObjectResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 404, ErrorMessage = "The given Id is not found" });
        }
        [Fact]
        public void UpdateAppointment_WhenTriedToUpdateOlderAppointment_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem)).Throws(new Exception("Cannot Update Older Appointment"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem);
            var badRequestResult = (BadRequestObjectResult)exceptionResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(exceptionResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Cannot Update Older Appointment" });
        }
        [Fact]
        public void UpdateAppointment_WhenTriedToUpdateAppointmentToPastTime_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = DateTime.Now.AddHours(-1), EndTime = DateTime.Now.AddHours(1) };
            Mock.Setup(service => service.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247471"), testItem)).Throws(new Exception("Cannot Update Appointment to Past time"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247471"), testItem);
            var badRequestResult = (BadRequestObjectResult)exceptionResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(exceptionResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Cannot Update Appointment to Past time" });
        }
        [Fact]
        public void UpdateAppointment_WhenAppointmentStartTimeAndEndTimeAreSame_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime(2022, 12, 12, 12, 12, 12), EndTime = new DateTime(2022, 12, 12, 12, 12, 12) };
            Mock.Setup(x => x.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem)).Throws(new Exception("Appointment Start time and End time should not be same"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem);
            var badRequestResult = (BadRequestObjectResult)exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Appointment Start time and End time should not be same" });
        }
        [Fact]
        public void UpdateAppointment_WhenAppointmentStartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            var testItem = new AddAppointment() { Description = "test", Title = "Demo", StartTime = new DateTime(2022, 12, 12, 12, 12, 12), EndTime = new DateTime(2022, 12, 12, 12, 12, 12) };
            Mock.Setup(x => x.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem)).Throws(new Exception("End time should be greater than Start time"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var exceptionResult = systemUnderTest.UpdateAppointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247481"), testItem);
            var badRequestResult = (BadRequestObjectResult)exceptionResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "End time should be greater than Start time" });
        }
        [Fact]
        public void GetAppointmentsByTitle_WhenTitlePassed_ReturnsOkResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(x=>x.GetAppointmentsByTitle("test", 1, 10, -330)).Returns(new PaginatedResponse());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointmentsByTitle( new SearchAppointmentQueryParameters(){Title="test", PageNumber=1, PageSize=10, TimeZoneOffset=-330} );
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
        [Fact]
        public void GetAppointmentsByTitle_WhenPageSizeGreaterThan100IsPassed_ReturnsOkResult()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(x=>x.GetAppointmentsByTitle("test", 1, 101, -330)).Returns(new PaginatedResponse());
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var okResult = systemUnderTest.GetAppointmentsByTitle( new SearchAppointmentQueryParameters(){Title="test", PageNumber=1, PageSize=101, TimeZoneOffset=-330} );
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
        [Fact]
        public void GetAppointmentsByTitle_WhenTitlePassedAsNull_ThrowsException()
        {
            //Arrange
            var Mock = new Mock<IAppointmentsBL>();
            Mock.Setup(x=>x.GetAppointmentsByTitle(null, 1, 10, -330)).Throws(new Exception("Title cannot be null"));
            var systemUnderTest = new AppointmentsController(Mock.Object);
            //Act
            var badRequestResult = systemUnderTest.GetAppointmentsByTitle( new SearchAppointmentQueryParameters(){Title=null, PageNumber=1, PageSize=10, TimeZoneOffset=-330} );
            var badRequestObjectResult = (BadRequestObjectResult)badRequestResult;
            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            badRequestObjectResult.Value.Should().BeEquivalentTo(new ErrorResponse() { StatusCode = 400, ErrorMessage = "Title cannot be null" });
        }
    }
}