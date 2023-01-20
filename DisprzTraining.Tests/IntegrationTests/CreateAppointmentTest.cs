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
using Appointments;
using System.Text;

namespace DisprzTraining.Tests.IntegrationTests
{
    public class CreateAppointmentIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
    private readonly WebApplicationFactory<Program> _factory;

    public CreateAppointmentIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
       [Fact]
        public async Task CreateAppointment_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="test",
              StartTime=new DateTime(2024, 10, 10, 10, 10, 10, 10),
              EndTime=new DateTime(2024, 10, 10, 11, 10, 10, 10),
              Description="test" ,           
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/appointments", stringContent);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task CreateAppointment_WhenAppointmentConflicts_ReturnsConflictResult()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="test",
              StartTime = new DateTime(2025, 09, 26, 05, 06, 07 ), 
              EndTime = new DateTime(2025, 09, 26, 06, 06, 07),
              Description="test" ,           
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/appointments", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task CreateAppointment_WhenStartTimeAndEndTimeAreSame_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="test",
              StartTime=new DateTime(2024, 10, 10, 12, 10, 10),
              EndTime=new DateTime(2024, 10, 10, 12, 10, 10),
              Description="test" ,           
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/appointments", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task CreateAppointment_WhenStartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="test",
              StartTime=new DateTime(2024, 10, 10, 14, 10, 10),
              EndTime=new DateTime(2024, 10, 10, 12, 10, 10),
              Description="test" ,           
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/appointments", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task CreateAppointment_WhenStartTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="test",
              StartTime=null,
              EndTime=new DateTime(2024, 10, 10, 12, 10, 10),
              Description="test" ,           
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/appointments", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task CreateAppointment_WhenEndTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="test",
              StartTime=new DateTime(2024, 10, 10, 12, 10, 10),
              EndTime=null,
              Description="test" ,           
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/appointments", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task CreateAppointment_WhenBothStartTimeAndEndTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="test",
              StartTime=null,
              EndTime=null,
              Description="test" ,           
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/appointments", stringContent);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}