using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using FluentAssertions;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
// using Appointments;
using DisprzTraining.Models;
using System.Text;

namespace DisprzTraining.Tests.IntegrationTests
{
    public class UpdateAppointmentIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UpdateAppointmentIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        //Update appointment Passing test case
        
        [Fact]
        public async Task UpdateAppointment_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = new DateTime(2024, 11, 10, 10, 10, 10, 10),
                EndTime = new DateTime(2024, 11, 10, 11, 10, 10, 10),
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247471", stringContent);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //Update appointment Failing test cases

        [Fact]
        public async Task UpdateAppointment_WhenStartTimeAndEndTimeAreSame_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = new DateTime(2024, 11, 10, 10, 10, 10, 10),
                EndTime = new DateTime(2024, 11, 10, 10, 10, 10, 10),
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247471", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task UpdateAppointment_WhenStartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = new DateTime(2024, 11, 10, 14, 10, 10, 10),
                EndTime = new DateTime(2024, 11, 10, 10, 10, 10, 10),
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247471", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task UpdateAppointment_WhenStartTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = null,
                EndTime = new DateTime(2024, 11, 10, 10, 10, 10, 10),
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247471", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task UpdateAppointment_WhenEndTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = new DateTime(2024, 11, 10, 10, 10, 10, 10),
                EndTime = null,
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247471", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task UpdateAppointment_WhenBothStartTimeAndEndTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = null,
                EndTime = null,
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247471", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task UpdateAppointment_WhenTriedToUpdateAppointmentToPastTime_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = new DateTime(2021, 11, 10, 10, 10, 10, 10),
                EndTime = new DateTime(2021, 11, 10, 11, 10, 10, 10),
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247471", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task UpdateAppointment_NotExistingIdPassed_ReturnsNotFound()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = new DateTime(2024, 11, 10, 10, 10, 10, 10),
                EndTime = new DateTime(2024, 11, 10, 11, 10, 10, 10),
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04241351", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task UpdateAppointment_WhenExistingIdPassedAndConflictOccurs_ReturnsConflict()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
                Title = "test",
                StartTime = new DateTime(2025, 08, 26, 05, 06, 07),
                EndTime = new DateTime(2025, 08, 26, 05, 26, 07),
                Description = "test"
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247482", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}