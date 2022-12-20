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


        [HttpGet("",Name = "Get All Appointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAppointments()
        {
            return Ok(await _appointmentsBL.GetAllAppointments());
        }

        [HttpGet("date/{appointmentDate}", Name ="Get Appointments by Date")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppointments(string appointmentDate)
        {
            var appointments = await _appointmentsBL.GetAppointments(appointmentDate);
            return Ok(appointments);   
        }
        [HttpGet("{id}", Name ="Get Appointment by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var appointment = await _appointmentsBL.GetAppointmentById(id);
            return Ok(appointment);   
        }



        [HttpPost(Name = "Create an Appointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAppointment([FromBody]Appointment appointment)
        {
            var result = await _appointmentsBL.AppointmentConflictCheck(appointment);
            if(!result){
                await _appointmentsBL.CreateAppointment(appointment);
            return Created("Appointment Created", true);
            }
            return Conflict();
        }

        [HttpDelete("{id}", Name = "Remove Appointment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var result= await _appointmentsBL.DeleteAppointment(id);
            if(result){
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("{id}", Name = "Update Appointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody]Appointment appointment)
        {
            var conflict = await _appointmentsBL.AppointmentConflictCheck(appointment);
            var result = await _appointmentsBL.UpdateAppointment(id, appointment);
            if(result && !conflict){
                return Ok();
            }
            else if(conflict){
                return Conflict();
            }
            
            return NotFound();
        }


    }
}
