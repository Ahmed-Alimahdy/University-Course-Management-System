using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.ApiService.DTOs
{
    public class UpdateInstructorDTO
    {
        [Required]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
        public string? Title { get; set; }
        public DateTime? HireDate { get; set; }
    }
}
