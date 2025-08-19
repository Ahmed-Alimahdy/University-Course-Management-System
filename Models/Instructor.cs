using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using universityManagementSys.CustomValidations;

namespace universityManagementSys.Models
{
    public class Instructor
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MinLength(2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        [MaxLength(50, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        [MaxLength(50, ErrorMessage = "last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Remote(action: "IsPhoneUnique", controller: "Instructor", AdditionalFields = "ID")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100)]
        [Remote(action: "IsEmailUnique", controller: "Instructor", AdditionalFields = "ID")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Hire date is required.")]
        [DataType(DataType.Date)]
        [HireDateValidation]
        public DateTime HireDate { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
