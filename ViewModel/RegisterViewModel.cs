using System.ComponentModel.DataAnnotations;
using universityManagementSys.Models;

namespace universityManagementSys.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "FirstName")]
        public string Firstname { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } // Will be "Student" or "Instructor"
        public Student student { get; set; }
        public Instructor instructor { get; set; }
    }

}