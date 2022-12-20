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
              Id=new Guid(),
              AppointmentTitle="test",
              AppointmentDate="2022-12-12",
              AppointmentStartTime=new DateTime(2021, 10, 10, 12, 10, 10),
              AppointmentEndTime=new DateTime(2021, 10, 10, 20, 10, 10),
              AppointmentDescription="test"             
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PostAsync("api/Appointments", stringContent);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}