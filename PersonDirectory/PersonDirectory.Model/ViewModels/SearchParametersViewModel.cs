using Microsoft.Extensions.Localization;
using PersonDirectory.DTO;

namespace PersonDirectory.Model.ViewModels
{
    public class SearchParametersViewModel
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public string? PersonalNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? CityId { get; set; }
        public IEnumerable<CityViewModel>? CityModels { get; set; }
        public IEnumerable<PersonViewModel>? Persons { get; set; }
        public IStringLocalizer Localizer { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
