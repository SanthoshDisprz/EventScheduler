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
using System.Net.Http.Json;

namespace DisprzTraining.Tests.IntegrationTests
{
    public class CreateAppointmentIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
    private readonly WebApplicationFactory<Program> _factory;

    public CreateAppointmentIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    //Testing all endpoints

    //Passing test case
       [Fact]
        public async Task CreateGetUpdateAndDeleteAppointment_ReturnSuccess_AndCorrectContentType()
        {
          //Creating an appointment

            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Title="Integration test",
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

          //Getting the created appointment

            //Act
            var getResponse = await client.GetAsync("api/appointments?from=2024-10-10T09%3A08%3A47.017Z&to=2024-10-10T12%3A12%3A47.017Z&timeZoneOffset=-330");
            //Assert
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
          
          //Searching the appointment by title
          //Act
            var getByTitleresponse = await client.GetAsync("api/appointments/search?title=Integration test&pageNumber=1&pageSize=10&timeZoneOffset=-330");
            //Assert
            getByTitleresponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, getByTitleresponse.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());

          //Updating the created appointment
            var appointmentData = getResponse.Content.ReadFromJsonAsync<List<Appointment>>();
            var appointmentId = appointmentData?.Result?.Find(x=>x.Title=="Integration test")?.Id;

            var updateMockData = new Appointment
            {
                Title = "Update test",
                StartTime = new DateTime(2026, 11, 10, 10, 10, 10, 10),
                EndTime = new DateTime(2026, 11, 10, 11, 10, 10, 10),
                Description = "test"
            };
            var serializeUpdateObject = JsonConvert.SerializeObject(updateMockData);
            var updateObjectStringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
             var updateResponse = await client.PutAsync($"api/appointments/{appointmentId}", updateObjectStringContent);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());

          //Deleting the appointment
             //Act
            var deleteResponse = await client.DeleteAsync($"api/appointments/{appointmentId}");
            //Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        }

        //Create appointment - Failing test cases
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