using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using FluentAssertions;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
// using Appointments;
using DisprzTraining.Models;
using System.Text;

namespace DisprzTraining.Tests.IntegrationTests
{
    public class DeleteAppointmentIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DeleteAppointmentIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        //Delete appointment Passing test case
        [Fact]
        public async Task DeleteAppointment_ReturnSuccess()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.DeleteAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247482");
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        //Delete appointment Failing test cases
        [Fact]
        public async Task DeleteAppointment_WhenTriedToDeletePastAppointment_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.DeleteAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247481");
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task DeleteAppointment_WhenNotExistingIdPassed_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.DeleteAsync("api/appointments/9245fe4a-d402-451c-b9ed-9c1a04247357");
            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}