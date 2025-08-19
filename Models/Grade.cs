using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace universityManagementSys.Models
{
    public class Grade
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Grade name is required")]
        [MaxLength(20, ErrorMessage = "Grade name cannot exceed 20 characters")]
        public string GradeName { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
