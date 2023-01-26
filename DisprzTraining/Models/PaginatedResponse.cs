using System;
using System.ComponentModel.DataAnnotations;
// using Appointments;

namespace DisprzTraining.Models
{
    public class PaginatedResponse
    {
        public int TotalPages { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
