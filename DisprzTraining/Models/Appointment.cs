using System;
using System.ComponentModel.DataAnnotations;
namespace Appointments
{
    public class Appointment
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        // public int TimeZoneOffset { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public List<string>? GuestsList {get; set; }
        public string? Location { get; set; }
    }
}
