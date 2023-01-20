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
        public async Task GetAppointments_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments?from=2023-01-17T11%3A08%3A47.017Z&to=2023-01-17T12%3A08%3A47.017Z&timeZoneOffset=-330");
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task GetAppointments_WhenFromTimeGreaterThanToTime_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments?from=2023-02-17T11%3A08%3A47.017Z&to=2023-01-17T12%3A08%3A47.017Z&timeZoneOffset=-330");
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task GetAppointments_WhenFromAndToTimeAreSame_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments?from=2023-02-17T11%3A08%3A47.017Z&to=2023-02-17T11%3A08%3A47.017Z&timeZoneOffset=-330");
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task GetAppointments_WhenFromTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments?to=2023-02-17T11%3A08%3A47.017Z&timeZoneOffset=-330");
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task GetAppointments_WhenToTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments?from=2023-02-17T11%3A08%3A47.017Z&timeZoneOffset=-330");
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }
        [Fact]
        public async Task GetAppointments_WhenBothFromAndToTimePassedAsNull_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("api/appointments?timeZoneOffset=-330");
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
        }

    }
}