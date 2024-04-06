using PersonDirectory.Model.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.Model
{
    public class CityModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "City name is required.")]
        [GeorgianOrLatinLetters]
        public string Name { get; set; }
    }
}
