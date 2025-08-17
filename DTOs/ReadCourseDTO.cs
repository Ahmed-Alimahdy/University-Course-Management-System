using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.ApiService.DTOs
{
    public class ReadCourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreditHours { get; set; }
        public string InstructorName { get; set; }
        public string SemesterName { get; set; }
    }
}
