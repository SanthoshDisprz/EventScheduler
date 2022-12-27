using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using FluentAssertions;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Appointments;
using System.Text;

namespace DisprzTraining.Tests.IntegrationTests
{
    public class GetAppointmentByIdIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
    private readonly WebApplicationFactory<Program> _factory;

    public GetAppointmentByIdIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
        [Fact]
        public async Task GetAppointmentById_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response =await client.GetAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247482");
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}