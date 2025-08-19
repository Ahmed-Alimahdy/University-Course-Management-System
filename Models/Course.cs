using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace universityManagementSys.Models
{
    public class Course
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [MinLength(3, ErrorMessage = "Course name must be between 3 and 100 characters")]
        [MaxLength(100, ErrorMessage = "Course name must be between 3 and 100 characters")]
        [Remote(action: "IsCourseNameUnique", controller: "Course", AdditionalFields = "ID", 
            ErrorMessage = "This course name already exists")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Range(1, 6, ErrorMessage = "Credit hours must be between 1 and 6")]
        public int CreditHours { get; set; }

        [Required(ErrorMessage = "Instructor is required")]
        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }

        [Required(ErrorMessage = "Semester is required")]
        public int SemesterID { get; set; }
        public Semester Semester { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<DepartmentCourse> DepartmentCourses { get; set; }

        public static implicit operator Course(Task<Course?> v)
        {
            throw new NotImplementedException();
        }
    }
}
