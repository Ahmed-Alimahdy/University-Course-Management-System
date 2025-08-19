using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using universityManagementSys.CustomValidations;

namespace universityManagementSys.Models
{
    public class Semester
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Semester name is required")]
        [MaxLength(50, ErrorMessage = "Semester name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        public DateTime EndDate { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
