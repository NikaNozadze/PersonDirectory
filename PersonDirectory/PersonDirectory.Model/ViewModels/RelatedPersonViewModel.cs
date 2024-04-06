using Microsoft.Extensions.Localization;
using PersonDirectory.DTO;

namespace PersonDirectory.Model.ViewModels
{
    public class RelatedPersonViewModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public PersonViewModel? Person { get; set; }
        public IEnumerable<PersonViewModel>? Persons { get; set; }
        public RelationType Type { get; set; }
        public int RelatedPersonId { get; set; }
        public PersonViewModel? RelatedPerson { get; set; }
        public IStringLocalizer Localizer { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
