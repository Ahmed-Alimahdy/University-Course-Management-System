using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace universityManagementSys.Models
{
    public class DepartmentCourse
    {
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

    }
}
