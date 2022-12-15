using System;
using System.ComponentModel.DataAnnotations;
namespace Appointments
{
    public class Appointment
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? AppointmentTitle { get; set; }
        [Required]
        public string? AppointmentDate { get; set; }
        [Required]
        public DateTime? AppointmentStartTime { get; set; }
        [Required]
        public DateTime? AppointmentEndTime { get; set; }
        public string? AppointmentDescription { get; set; }
    }
}
