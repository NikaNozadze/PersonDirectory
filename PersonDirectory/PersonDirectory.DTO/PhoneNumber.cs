using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.DTO
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        [Required]
        public PhoneType Type { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Number { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }
    }

    public enum PhoneType : byte
    {
        Mobile,
        Office,
        Home
    }
}
