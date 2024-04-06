using PersonDirectory.DTO;
using PersonDirectory.Model.Attributes;
using PersonDirectory.Model.ViewModels;

namespace PersonDirectory.Model
{
    public class SearchParametersModel
    {
        public int? Id { get; set; }
        [GeorgianOrLatinLetters]
        public string? FirstName { get; set; }
        [GeorgianOrLatinLetters]
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public string? PersonalNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? CityId { get; set; }
        public IEnumerable<PersonViewModel>? Persons { get; set; }
    }
}
