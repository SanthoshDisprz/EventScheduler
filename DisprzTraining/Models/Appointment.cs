using System;
using System.ComponentModel.DataAnnotations;
namespace DisprzTraining.Models
{
    public class Appointment : AddAppointment
    {
        [Required]
        public Guid Id { get; set; }
    }
}
