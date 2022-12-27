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


        [HttpGet("",Name = "Get All Appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Appointment>))]
        public async Task<IActionResult> GetAllAppointments()
        {
            return Ok(await _appointmentsBL.GetAllAppointments());
        }

        [HttpGet("date/{appointmentDate}", Name ="Get Appointments by Date")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Appointment>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetAppointmentsByDate(string appointmentDate)
        {
            try{
            var appointments = await _appointmentsBL.GetAppointmentsByDate(appointmentDate);
            return Ok(appointments);   
            }
            catch(Exception ex){
                return BadRequest(new ErrorResponse(){StatusCode=400, ErrorMessage=ex.Message});
            }
        }
        [HttpGet("{id}", Name ="Get Appointment by Id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Appointment>))]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var appointment = await _appointmentsBL.GetAppointmentById(id);
            return Ok(appointment);   
        }



        [HttpPost(Name = "Create an Appointment")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAppointment([FromBody]Appointment appointment)
        {
            try
            {
            var result = await _appointmentsBL.AppointmentConflictCheck(appointment);
            if(!result){
                await _appointmentsBL.CreateAppointment(appointment);
            return Created("~api/Appointments", true);
            }
            return Conflict(new ErrorResponse(){StatusCode=409, ErrorMessage="Appointment Conflict Occured."});
            }
             catch(Exception ex){
                return BadRequest(new ErrorResponse(){StatusCode=400, ErrorMessage=ex.Message});
            }
        }

        [HttpDelete("{id}", Name = "Remove Appointment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            try{
            var result= await _appointmentsBL.DeleteAppointment(id);
            if(result){
                return NoContent();
            }
            return NotFound(new ErrorResponse(){StatusCode=404, ErrorMessage="The given Id is not found"});
            }
             catch(Exception ex){
                return BadRequest(new ErrorResponse(){StatusCode=400, ErrorMessage=ex.Message});
            }
           
        }

        [HttpPut("{id}", Name = "Update Appointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody]UpdateAppointment appointment)
        {
            try
            {
            var conflict = await _appointmentsBL.UpdateAppointmentConflictCheck(id, appointment);
            var result = await _appointmentsBL.UpdateAppointment(id, appointment);
            if(result && !conflict){
                return Ok();
            }
            else if(conflict){
                return Conflict(new ErrorResponse(){StatusCode=409, ErrorMessage="Appointment Conflict Occured."});
            }
            
            return NotFound(new ErrorResponse(){StatusCode=404, ErrorMessage="The given Id is not found"});
            }
            catch(Exception ex){
                return BadRequest(new ErrorResponse(){StatusCode=400, ErrorMessage=ex.Message});
            }
        }


    }
}
