using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisprzTraining.Models
{
    public class GetAppointmentsQueryParameters
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int TimeZoneOffset { get; set; }
    }
}