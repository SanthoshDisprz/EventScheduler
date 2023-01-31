using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DisprzTraining.Models
{
    public class SearchAppointmentQueryParameters 
    {
        const int maxPageSize = 100;
        [Required]
        public string? Title { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }  
        public int TimeZoneOffset { get; set; }

    }
}