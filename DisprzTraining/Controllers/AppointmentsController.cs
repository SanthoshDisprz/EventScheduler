using DisprzTraining.Business;
// using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Appointments;
using DisprzTraining.Data;
using System.Threading.Tasks;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Cors;


namespace DisprzTraining.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsBL _appointmentsBL;
        public AppointmentsController(IAppointmentsBL appointmentsBL)
        {
            _appointmentsBL = appointmentsBL;
        }


        [HttpGet(Name = "Get Appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Appointment>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public IActionResult GetAppointments([FromQuery(Name = "from")] DateTime? startTime, [FromQuery(Name = "to")] DateTime? endTime, [FromQuery] int timeZoneOffset)
        {
            try
            {
                var appointments = _appointmentsBL.GetAppointments(startTime, endTime, timeZoneOffset);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { StatusCode = 400, ErrorMessage = ex.Message });
            }
        }


        [HttpPost(Name = "Create an Appointment")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public IActionResult CreateAppointment([FromBody] AddAppointment appointment)
        {
            try
            {
                var result = _appointmentsBL.CreateAppointment(appointment);
                if (result)
                {
                    return Created("~api/Appointments", true);
                }
                return Conflict(new ErrorResponse() { StatusCode = 409, ErrorMessage = "Appointment Conflict Occured." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { StatusCode = 400, ErrorMessage = ex.Message });
            }
        }

        [HttpDelete("{id}", Name = "Remove Appointment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public IActionResult DeleteAppointment(Guid id)
        {
            try
            {
                var result = _appointmentsBL.DeleteAppointment(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound(new ErrorResponse() { StatusCode = 404, ErrorMessage = "The given Id is not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { StatusCode = 400, ErrorMessage = ex.Message });
            }

        }

        [HttpPut("{id}", Name = "Update Appointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public IActionResult UpdateAppointment(Guid id, [FromBody] AddAppointment appointment)
        {
            try
            {
                var result = _appointmentsBL.UpdateAppointment(id, appointment);
                if (result)
                {
                    var conflict = _appointmentsBL.UpdateAppointmentConflictCheck(id, appointment);
                    if (conflict)
                    {
                        return Conflict(new ErrorResponse() { StatusCode = 409, ErrorMessage = "Appointment Conflict Occured." });
                    }
                    return Ok();

                }
                return NotFound(new ErrorResponse() { StatusCode = 404, ErrorMessage = "The given Id is not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { StatusCode = 400, ErrorMessage = ex.Message });
            }
        }


    }
}
