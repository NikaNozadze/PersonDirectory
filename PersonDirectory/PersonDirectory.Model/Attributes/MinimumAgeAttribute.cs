using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.Model.Attributes
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var dateOfBirth = (DateTime)value;
                if (dateOfBirth.AddYears(_minimumAge) > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? $"The age must be at least {_minimumAge} years.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
