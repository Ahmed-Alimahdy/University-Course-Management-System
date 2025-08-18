using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.DTOs.Courses
{
    public class UpdateCourseDTO
    {
        [Required]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CreditHours { get; set; }

        public int? InstructorID { get; set; }
        public int? SemesterID { get; set; }
    }
}
