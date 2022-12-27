using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisprzTraining.Models
{
    public class UpdateAppointment
    {
        [Required]
         public string? AppointmentTitle { get; set; }
        [Required]
        public string? AppointmentDate { get; set; }
        [Required]
        public DateTime? AppointmentStartDateTime { get; set; }
        [Required]
        public DateTime? AppointmentEndDateTime { get; set; }
        public string? AppointmentDescription { get; set; }
    }
}