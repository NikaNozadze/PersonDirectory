using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.DTO
{
    public class RelatedPersons
    {
        public int Id { get; set; }

        [Required]
        public RelationType Type { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public int RelatedPersonId { get; set; }

        public Person RelatedPerson { get; set; }
    }

    public enum RelationType : byte
    {
        Colleague,
        Acquaintance,
        Relative,
        Other
    }
}
