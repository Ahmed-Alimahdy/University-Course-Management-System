using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace universityManagementSys.Models
{
    public class Enrollment
    {
        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public int? GradeID { get; set; }
        public Grade Grade { get; set; }
    }
}
