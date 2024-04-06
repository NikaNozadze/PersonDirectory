using Microsoft.Extensions.Localization;

namespace PersonDirectory.Model.ViewModels
{
    public class CityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IStringLocalizer Localizer { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
