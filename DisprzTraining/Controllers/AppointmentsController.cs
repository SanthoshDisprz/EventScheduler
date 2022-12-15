using DisprzTraining.Business;
// using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Appointments;
using DisprzTraining.Data;
using System.Threading.Tasks;

namespace DisprzTraining.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsBL _appointmentsBL;
        public AppointmentsController(IAppointmentsBL appointmentsBL)
        {
            _appointmentsBL = appointmentsBL;
        }


        [HttpGet(Name = "Get All Appointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAppointments()
        {
            return Ok(await _appointmentsBL.GetAllAppointments());
        }

        [HttpGet("{eventDate}", Name ="Get Appointments by Date")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointments(string eventDate)
        {
            var appointments = await _appointmentsBL.GetAppointments(eventDate);
            if(appointments.Count>0){
                return Ok(appointments);
            }
            return NotFound();    
        }



        [HttpPost(Name = "Create an Appointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAppointment([FromBody]Appointment appointment)
        {
            var result = await _appointmentsBL.CreateAppointment(appointment);
            if(result){
            return Created("Appointment Created", appointment);
            }
            return Conflict();
        }

        [HttpDelete("{id}", Name = "Remove Appointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result= await _appointmentsBL.DeleteAppointment(id);
            if(result){
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("{id}", Name = "Update Appointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody]Appointment appointment)
        {
            var result = await _appointmentsBL.UpdateAppointment(id, appointment);
            if(result){
                return Ok();
            }
            return NotFound();
        }


    }
}
