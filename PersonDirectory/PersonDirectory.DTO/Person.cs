using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.DTO
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]

        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(11)]
        public string PersonalNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public int CityId { get; set; }

        public City? City { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<PhoneNumber>? PhoneNumbers { get; set; }

        public ICollection<RelatedPersons>? RelatedPeople { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
