using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.CustomValidations
{
    public class HireDateValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime hireDate && hireDate > DateTime.Today)
            {
                return new ValidationResult("Hire date cannot be in the future.");
            }

            return ValidationResult.Success;
        }
    }

}
