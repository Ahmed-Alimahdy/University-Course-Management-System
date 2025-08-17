using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.DTOs
{
    public class CreateCourseDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public int CreditHours { get; set; }

        public int? InstructorID { get; set; }
        public int? SemesterID { get; set; }
    }
}
