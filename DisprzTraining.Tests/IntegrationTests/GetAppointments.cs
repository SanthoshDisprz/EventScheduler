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
    public class GetAppointmentsIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
    private readonly WebApplicationFactory<Program> _factory;

    public GetAppointmentsIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    [Fact]
        public async Task GetAppointmentsWhenDatePassed_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response =await client.GetAsync("api/appointments?date=2023-01-12T10%3A19%3A09.108Z&timeZoneOffset=-330");
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task GetAppointmentsWhenDurationPassed_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response =await client.GetAsync("api/appointments?duration=Week");
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}