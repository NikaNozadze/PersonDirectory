using PersonDirectory.DTO;
using PersonDirectory.Model.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.Model
{
    public class PersonModel
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        [GeorgianOrLatinLetters]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        [GeorgianOrLatinLetters]
        public string LastName { get; init; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; init; }

        [Required(ErrorMessage = "Personal number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The personal number must contain 11 digits.")]
        public string PersonalNumber { get; init; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [MinimumAge(18, ErrorMessage = "Must be at least 18 years of age.")]
        public DateTime DateOfBirth { get; init; }

        [Required(ErrorMessage = "City ID is required.")]
        public int CityId { get; init; }

        public CityModel? City { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<PhoneNumberModel>? PhoneNumbers { get; set; }

        public ICollection<RelatedPersonModel>? RelatedPeople { get; set; }
    }
}
