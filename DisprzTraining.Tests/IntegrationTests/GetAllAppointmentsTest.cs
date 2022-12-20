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
    public class GetAllAppointmentsIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
    private readonly WebApplicationFactory<Program> _factory;

    public GetAllAppointmentsIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
        [Fact]
        public async Task GetAllAppointments_ReturnSuccess_AndCorrectContentType()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response =await client.GetAsync("api/Appointments");
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}
