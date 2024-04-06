using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PersonDirectory.Model.Attributes
{
    public class GeorgianOrLatinLetters : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string input = value.ToString();

                Regex georgianRegex = new(@"^[\u10A0-\u10FF\s]+$");
                Regex latinRegex = new(@"^[A-Za-z\s]+$");

                if (!georgianRegex.IsMatch(input) && !latinRegex.IsMatch(input))
                {
                    throw new ArgumentException("Input must contain only Georgian or Latin letters.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
