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
    public class UpdateAppointmentIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
    private readonly WebApplicationFactory<Program> _factory;

    public UpdateAppointmentIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
        [Fact]
        public async Task UpdateAppointment_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            var mockData = new Appointment
            {
              Id=new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
              Title="test",
              StartTime=DateTime.Now,
              EndTime=DateTime.Now.AddHours(1),
              Description="test"             
            };
            var serializeObject = JsonConvert.SerializeObject(mockData);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //Act
            var response =await client.PutAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247482", stringContent);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}