using Microsoft.Extensions.Localization;
using PersonDirectory.DTO;

namespace PersonDirectory.Model.ViewModels
{
    public class PhoneNumberViewModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public PersonViewModel? Person { get; set; }
        public IEnumerable<PersonViewModel>? Persons { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public IStringLocalizer Localizer { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
