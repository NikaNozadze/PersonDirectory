using Microsoft.Extensions.Localization;
using PersonDirectory.DTO;

namespace PersonDirectory.Model.ViewModels
{
    public class PersonViewModel
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public Gender Gender { get; init; }
        public string PersonalNumber { get; init; }
        public DateTime DateOfBirth { get; init; }
        public int CityId { get; init; }
        public string CityName { get; init; }
        public IEnumerable<CityViewModel>? CityModels { get; set; }
        public CityViewModel? City { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<PhoneNumberViewModel>? PhoneNumbers { get; set; }
        public ICollection<RelatedPersonViewModel>? RelatedPeople { get; set; }
        public IStringLocalizer Localizer { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
