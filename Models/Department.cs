using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace universityManagementSys.Models
{
    public class Department
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [MinLength(3, ErrorMessage = "Department name must be between 3 and 100 characters.")]
        [MaxLength(100, ErrorMessage = "Department name must be between 3 and 100 characters.")]
        [Remote(action: "IsDepartmentNameUnique", controller: "Department", AdditionalFields = "ID",
            ErrorMessage = "This department name already exists.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<DepartmentCourse> DepartmentCourses { get; set; }
    }
}
