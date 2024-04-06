using PersonDirectory.DTO;
using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.Model
{
    public class PhoneNumberModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Phone type is required.")]
        public PhoneType Type { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Phone number must be between 4 and 50 characters.")]
        public string Number { get; set; }

        public int PersonId { get; set; }
    }
}
