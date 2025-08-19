using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.CustomValidations
{
    public class CustomAgeValidation : ValidationAttribute
    {
        private readonly int _minAge;

        public CustomAgeValidation(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dob)
            {
                var age = DateTime.Today.Year - dob.Year;
                if (dob.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < _minAge)
                {
                    return new ValidationResult(ErrorMessage ?? $"Age must be at least {_minAge}");
                }
            }

            return ValidationResult.Success;
        }
    }

}
