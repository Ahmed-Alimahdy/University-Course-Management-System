using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace universityManagementSys.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreditHours { get; set; }

        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }

        public int SemesterID { get; set; }
        public Semester Semester { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<DepartmentCourse> DepartmentCourses { get; set; }
    }
}
