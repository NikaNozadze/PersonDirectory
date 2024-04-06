using Microsoft.Extensions.Localization;

namespace PersonDirectory.Model.ViewModels
{
    public class HomeViewModel
    {
        public string? CurrentCulture { get; set; }
        public string? CultureName { get; set; }
        public string? SearchTerm { get; set; }
        public IEnumerable<PersonViewModel>? SearchedPersons { get; set; }
        public IEnumerable<CityViewModel>? SearchedCities { get; set; }
        public IEnumerable<PhoneNumberViewModel>? SearchedPhoneNumbers { get; set; }
        public IEnumerable<RelatedPersonViewModel>? SearchedRelatedPersons { get; set; }
        public SearchType SearchType { get; set; }
        public IStringLocalizer Localizer { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public enum SearchType
    {
        Person,
        City,
        PhoneNumber
    }
}
