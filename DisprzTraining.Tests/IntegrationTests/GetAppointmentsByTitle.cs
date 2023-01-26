using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DisprzTraining.Tests.IntegrationTests
{
    public class GetAppointmentsByTitleIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GetAppointmentsByTitleIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        //Get appointments by title Passing test case
        [Fact]
        public async Task GetAppointmentsByTitle_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments/search?title=test&pageNumber=1&pageSize=10&timeZoneOffset=-330");
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }
        //Get appointments by title Failing test case
        [Fact]
        public async Task GetAppointmentsByTitle_WhenTitlePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments/search?pageNumber=1&pageSize=10&timeZoneOffset=-330");
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}