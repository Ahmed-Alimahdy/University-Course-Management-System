using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace universityManagementSys.Models
{
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<DepartmentCourse> DepartmentCourses { get; set; }
    }
}
