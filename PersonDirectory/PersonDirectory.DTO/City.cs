using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.DTO
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
