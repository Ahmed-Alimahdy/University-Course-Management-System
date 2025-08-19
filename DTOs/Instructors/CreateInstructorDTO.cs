using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.DTOs.Instructors
{
    public class CreateInstructorDTO
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        public string PhoneNum { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
    }
}
