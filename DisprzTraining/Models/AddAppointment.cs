using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisprzTraining.Models
{
    public class AddAppointment
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Appointment title is mandatory")]
        public string Title { get; set; } 
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public List<string>? GuestsList {get; set; }
        public string? Location { get; set; }
    }
}